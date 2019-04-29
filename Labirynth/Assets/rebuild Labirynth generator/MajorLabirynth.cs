using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MajorLabirynth : MonoBehaviour
{
    MajorCell[,] majorLabirynthGrid;
    GameObject[,] majorCellObjectsGrid;

    

    [SerializeField]
    int MajorLabirynthDimmension;
    [SerializeField]
    int MinorLabirynthDimmension;

    [SerializeField]
    float MinorSize;

    float MajorSize;

    
    float globalSize;

    
    

    [SerializeField]
    GameObject majorCellPrefab;

    

    [SerializeField]
    RandomNumbersGenerator randomNumbersGenerator;

    [SerializeField]
    [Range(0, 100)]
    int repeatChance = 50;

    bool generatingDone = false;

    IntVector2 cursor;

    // Start is called before the first frame update
    void Start()
    {
        if (MajorLabirynthDimmension % 2 == 0) MajorLabirynthDimmension++;
        if (MinorLabirynthDimmension % 2 == 0) MinorLabirynthDimmension++;


        MajorSize = MinorLabirynthDimmension * MinorSize;
        globalSize = MinorLabirynthDimmension * MinorSize * MajorLabirynthDimmension;

        

        Generate(new IntVector2(1, 1)); //execute from outer source
        

    }

    public void Generate(IntVector2 _startCursor)
    {
        cursor = _startCursor;

        majorCellObjectsGrid = new GameObject[MajorLabirynthDimmension, MajorLabirynthDimmension];
        majorLabirynthGrid = new MajorCell[MajorLabirynthDimmension, MajorLabirynthDimmension];

        

        Thread generator = new Thread(Generating);
        generator.Start();

    }

    private void Generating()
    {
        for (int y = 0; y < MajorLabirynthDimmension; y++)
        {
            for (int x = 0; x < MajorLabirynthDimmension; x++)
            {
                if(x%2 != 0 && y%2 != 0)
                {
                    majorLabirynthGrid[x, y] = new MajorCell(new IntVector2(x, y), MajorCell.CELL_TYPE.EMPTY);
                    continue;
                }
                else if(x%2 == 0 && y%2 == 0)
                {
                    majorLabirynthGrid[x, y] = new MajorCell(new IntVector2(x, y), MajorCell.CELL_TYPE.OBSTICLE);
                    continue;
                }
                majorLabirynthGrid[x, y] = new MajorCell(new IntVector2(x, y), MajorCell.CELL_TYPE.WALL);

            }
        }


        List<MajorCell> walkedCells = new List<MajorCell>();
        walkedCells.Add(majorLabirynthGrid[cursor.x, cursor.y]);
        majorLabirynthGrid[cursor.x, cursor.y].type = MajorCell.CELL_TYPE.PATH;



        

        int c = 1000;
        while (walkedCells.Count > 0 && c > 0)
        {
            int repeat = randomNumbersGenerator.GetRandomNumber(0, 101);
            int randomWalked;
            if (repeat < repeatChance)
            {
                randomWalked = walkedCells.Count - 1;
                cursor = walkedCells[randomWalked].position;
            }
            else
            {
                randomWalked = randomNumbersGenerator.GetRandomNumber(0, walkedCells.Count);
                cursor = walkedCells[randomWalked].position;
            }


            List<MajorCell> neighbours = GetNeighbours(cursor);

            

            if (neighbours.Count <= 0)
            {
                walkedCells.RemoveAt(randomWalked);
                continue;
            }

            int randomNeighbour = randomNumbersGenerator.GetRandomNumber(0, neighbours.Count);

            walkedCells.Add(neighbours[randomNeighbour]);

            majorLabirynthGrid[neighbours[randomNeighbour].position.x, neighbours[randomNeighbour].position.y].type = MajorCell.CELL_TYPE.PATH;

            IntVector2 w = new IntVector2((cursor.x + neighbours[randomNeighbour].position.x) / 2, (cursor.y + neighbours[randomNeighbour].position.y) / 2);

            majorLabirynthGrid[w.x, w.y].type = MajorCell.CELL_TYPE.PATH;
            
            cursor = walkedCells[walkedCells.Count - 1].position;




            c--;

        }

        



        generatingDone = true;
        Debug.Log("major labirynth generating done!");
    }

    private List<MajorCell> GetNeighbours(IntVector2 location)
    {
        List<MajorCell> neighbours = new List<MajorCell>();

        IntVector2 dir = new IntVector2();

        for (int i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0:
                    dir = new IntVector2(location.x + 2, location.y);
                    break;
                case 1:
                    dir = new IntVector2(location.x - 2, location.y);
                    break;
                case 2:
                    dir = new IntVector2(location.x, location.y + 2);
                    break;
                case 3:
                    dir = new IntVector2(location.x, location.y - 2);
                    break;
            }



            if (dir.x < 0 || dir.x >= MajorLabirynthDimmension || dir.y < 0 || dir.y >= MajorLabirynthDimmension) continue;
            
            if (majorLabirynthGrid[dir.x, dir.y].type == MajorCell.CELL_TYPE.WALL || majorLabirynthGrid[dir.x, dir.y].type == MajorCell.CELL_TYPE.OBSTICLE || majorLabirynthGrid[dir.x, dir.y].type == MajorCell.CELL_TYPE.PATH) continue;

            neighbours.Add(majorLabirynthGrid[dir.x, dir.y]);
        }

        return neighbours;

    }


    // Update is called once per frame
    void Update()
    {
        

        if (generatingDone)
        {
            generatingDone = false;
            Spawn();
        }
    }

    private void Spawn()
    {
        for (int y = 0; y < MajorLabirynthDimmension; y++)
        {
            for (int x = 0; x < MajorLabirynthDimmension; x++)
            {
                //spawning minor objects
                //wait to build after generating major labirynth

                //for now just spawning example objects with their own generators
                Vector3 position = new Vector3(transform.position.x - (globalSize / 2) + (x * MajorSize) + (MajorSize / 2), transform.position.y - (globalSize / 2) + (y * MajorSize) + (MajorSize / 2), 0);

                
                majorCellObjectsGrid[x, y] = (GameObject)Instantiate(majorCellPrefab, position, Quaternion.Euler(0, 0, 0));
                majorCellObjectsGrid[x, y].transform.localScale = new Vector3(MajorSize, MajorSize, 0);
                majorCellObjectsGrid[x, y].transform.parent = transform;
                majorCellObjectsGrid[x, y].GetComponent<MajorCellObject>().Initialize(MinorLabirynthDimmension, MinorSize, MajorSize, majorLabirynthGrid[x, y].type);



            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position/* - new Vector3(globalSize / 2, globalSize / 2, 0)*/, new Vector3(globalSize, globalSize, 0));
    }
}
