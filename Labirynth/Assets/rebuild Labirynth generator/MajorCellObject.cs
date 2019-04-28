using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MajorCellObject : RebuildCellObject
{
    GameObject[,] minorCellObjectsGrid;
    MajorCell[,] minorLabirynthGrid;

    [SerializeField]
    GameObject minorCellPrefab;

    [SerializeField]
    SpriteRenderer renderer;

    [SerializeField]
    List<Sprite> sprites;

    [SerializeField]
    RandomNumbersGenerator randomNumbersGenerator;

    [SerializeField]
    [Range(0, 100)]
    int repeatChance = 50;

    bool generatingDone = false;


    float minorSize;
    int minorDimension;

    float majorSize;
    int majorDimension;

    IntVector2 cursor;

    private void Start()
    {
        
        
    }

    public void Initialize(int MinorDimension, float MinorSize, float MajorSize, MajorCell.CELL_TYPE type)
    {
        minorSize = MinorSize;
        minorDimension = MinorDimension;

        majorSize = MajorSize;
        

        size = new Vector3(MajorSize, MajorSize, 0);

        randomNumbersGenerator = GameObject.FindGameObjectWithTag("RandomNumbersGenerator").GetComponent<RandomNumbersGenerator>();

        switch (type)
        {
            case MajorCell.CELL_TYPE.EMPTY:
                
                renderer.sprite = sprites[0];
                break;
            case MajorCell.CELL_TYPE.OBSTICLE:
                renderer.sprite = sprites[0];
                break;
            case MajorCell.CELL_TYPE.PATH:
                //renderer.sprite = sprites[2];
                minorLabirynthGrid = new MajorCell[minorDimension, minorDimension];
                Thread generator = new Thread(Generating);
                generator.Start();
                break;
            case MajorCell.CELL_TYPE.WALL:
                renderer.sprite = sprites[0];
                break;
        }

        //transform.localScale = new Vector3(MajorSize, MajorSize, 1);

        return; //blocking for a moment

        minorCellObjectsGrid = new GameObject[MinorDimension, MinorDimension];

        for (int y = 0; y < MinorDimension; y++)
        {
            for (int x = 0; x < MinorDimension; x++)
            {
                Vector3 position = new Vector3(transform.position.x + (x * MinorSize) - MajorSize / 2 + MinorSize / 2, transform.position.y + (y * MinorSize) - MajorSize / 2 + MinorSize / 2, 0);
                minorCellObjectsGrid[x, y] = (GameObject)Instantiate(minorCellPrefab, position, Quaternion.Euler(0, 0, 0), this.transform);
                minorCellObjectsGrid[x, y].transform.localScale = new Vector3(MinorSize, MinorSize, 1);
                
            }
        }

        
    }

    private void Generating()
    {
        for (int y = 0; y < minorDimension; y++)
        {
            for (int x = 0; x < minorDimension; x++)
            {
                if (x % 2 != 0 && y % 2 != 0)
                {
                    minorLabirynthGrid[x, y] = new MajorCell(new IntVector2(x, y), MajorCell.CELL_TYPE.EMPTY);
                    continue;
                }
                else if (x % 2 == 0 && y % 2 == 0)
                {
                    minorLabirynthGrid[x, y] = new MajorCell(new IntVector2(x, y), MajorCell.CELL_TYPE.OBSTICLE);
                    continue;
                }
                minorLabirynthGrid[x, y] = new MajorCell(new IntVector2(x, y), MajorCell.CELL_TYPE.WALL);

            }
        }

        cursor = new IntVector2(1, 1);

        List<MajorCell> walkedCells = new List<MajorCell>();
        walkedCells.Add(minorLabirynthGrid[cursor.x, cursor.y]);
        minorLabirynthGrid[cursor.x, cursor.y].type = MajorCell.CELL_TYPE.PATH;


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

            minorLabirynthGrid[neighbours[randomNeighbour].position.x, neighbours[randomNeighbour].position.y].type = MajorCell.CELL_TYPE.PATH;

            IntVector2 w = new IntVector2((cursor.x + neighbours[randomNeighbour].position.x) / 2, (cursor.y + neighbours[randomNeighbour].position.y) / 2);

            minorLabirynthGrid[w.x, w.y].type = MajorCell.CELL_TYPE.PATH;

            cursor = walkedCells[walkedCells.Count - 1].position;




            c--;

        }


        Debug.Log("minor labirynth generating done!");
        generatingDone = true;
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



            if (dir.x < 0 || dir.x >= minorDimension || dir.y < 0 || dir.y >= minorDimension) continue;

            if (minorLabirynthGrid[dir.x, dir.y].type == MajorCell.CELL_TYPE.WALL || minorLabirynthGrid[dir.x, dir.y].type == MajorCell.CELL_TYPE.OBSTICLE || minorLabirynthGrid[dir.x, dir.y].type == MajorCell.CELL_TYPE.PATH) continue;

            neighbours.Add(minorLabirynthGrid[dir.x, dir.y]);
        }

        return neighbours;

    }

    void Update()
    {
        if (generatingDone)
        {
            Spawn();
            generatingDone = false;
        }
    }

    private void Spawn()
    {
        minorCellObjectsGrid = new GameObject[minorDimension, minorDimension];

        for (int y = 0; y < minorDimension; y++)
        {
            for (int x = 0; x < minorDimension; x++)
            {


                Vector3 position = new Vector3(transform.position.x - (majorSize / 2) + (x * minorSize) + (minorSize / 2), transform.position.y - (majorSize / 2) + (y * minorSize) + (minorSize / 2), 0);

                minorCellObjectsGrid[x, y] = (GameObject)Instantiate(minorCellPrefab, position, Quaternion.Euler(0, 0, 0));
                minorCellObjectsGrid[x, y].transform.localScale = new Vector3(minorSize, minorSize, 0);
                minorCellObjectsGrid[x, y].GetComponent<MinorCellObject>().Initialize(minorSize, minorLabirynthGrid[x, y].type);
                minorCellObjectsGrid[x, y].transform.parent = transform;

                
            }
        }
    }
}
