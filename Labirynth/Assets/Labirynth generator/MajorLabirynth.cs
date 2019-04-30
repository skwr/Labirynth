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
    float MinorSize = 1;

    float MajorSize;
    
    float globalSize;
    
    [SerializeField]
    GameObject majorCellPrefab = null;

    [SerializeField]
    RandomNumbersGenerator randomNumbersGenerator = null;

    [SerializeField]
    [Range(0, 100)]
    int repeatChance = 50;  //chance for generator to make step from current cursor location or random from walked positions

    bool generatingDone = false;    //flag to let this object spawn generated labirynth

    IntVector2 cursor;  //variable to hold position of current generator step

    // Start is called before the first frame update
    void Start()
    {
        //increase dimensions if they aren`t odd number
        if (MajorLabirynthDimmension % 2 == 0) MajorLabirynthDimmension++;
        if (MinorLabirynthDimmension % 2 == 0) MinorLabirynthDimmension++;

        //calculate major object size and labirynth global size in space
        MajorSize = MinorLabirynthDimmension * MinorSize;
        globalSize = MinorLabirynthDimmension * MinorSize * MajorLabirynthDimmension;

        
        Generate(new IntVector2(1, 1)); //make execute from outer source
        

    }

    //method that is starting new thread for generating
    public void Generate(IntVector2 _startCursor)
    {
        cursor = _startCursor;

        majorCellObjectsGrid = new GameObject[MajorLabirynthDimmension, MajorLabirynthDimmension];
        majorLabirynthGrid = new MajorCell[MajorLabirynthDimmension, MajorLabirynthDimmension];

        

        Thread generator = new Thread(Generating);
        generator.Start();

    }

    //method executed on thread
    private void Generating()
    {
        //filling grid width basic objects
        //place spots that will be always walked (EMPTY for beggining), spots that will be always wall (OBSTICLE) and spots that will be changable (WALL)
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

        //preparing list of walked cells
        //generator is choosing one of this elements to make next step
        List<MajorCell> walkedCells = new List<MajorCell>();

        //inserting first element to list, it is that element what is cursor pointing at
        majorLabirynthGrid[cursor.x, cursor.y].type = MajorCell.CELL_TYPE.PATH;
        walkedCells.Add(majorLabirynthGrid[cursor.x, cursor.y]);



        //generator starting

        int c = 1000;   //"timeout" variable, in case that something go wrong
        while (walkedCells.Count > 0 && c > 0)
        {
            int repeat = randomNumbersGenerator.GetRandomNumber(0, 101);     //get random number to draw if generator should make next step from last cursor position or draw new position from walkedCells
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

            //get all neighbours of spot that is pointed by curosr
            List<MajorCell> neighbours = GetNeighbours(cursor);

            List<MajorCell> neighboursForRandomSelect = new List<MajorCell>();
            for (int i = 0; i < neighbours.Count; i++)
            {
                if (neighbours[i] == null) continue;
                neighboursForRandomSelect.Add(neighbours[i]);
            }

            


            //remove this spot form walked if it haven`t got any neighbour
            if (neighboursForRandomSelect.Count <= 0)
            {
                walkedCells.RemoveAt(randomWalked);
                continue;
            }

            //get random neighbour index from list
            int randomNeighbour = randomNumbersGenerator.GetRandomNumber(0, neighboursForRandomSelect.Count);

            //inserting selected neighbour int
            walkedCells.Add(neighboursForRandomSelect[randomNeighbour]);
            majorLabirynthGrid[neighboursForRandomSelect[randomNeighbour].position.x, neighboursForRandomSelect[randomNeighbour].position.y].type = MajorCell.CELL_TYPE.PATH;

            //changing object omitted on generator step
            IntVector2 w = new IntVector2((cursor.x + neighboursForRandomSelect[randomNeighbour].position.x) / 2, (cursor.y + neighboursForRandomSelect[randomNeighbour].position.y) / 2);
            majorLabirynthGrid[w.x, w.y].type = MajorCell.CELL_TYPE.PATH;

            int randomDoorIndex = randomNumbersGenerator.GetRandomNumber(3, MinorLabirynthDimmension - 3);
            if (randomDoorIndex % 2 == 0) randomDoorIndex++;


            int dir = -1;

            for(int i = 0; i < neighbours.Count; i++)
            {
                if(neighbours[i] == neighboursForRandomSelect[randomNeighbour])
                {
                    dir = i;
                    break;
                }
            }
            
            switch (dir)
            { 
                case 0:
                    //right
                    majorLabirynthGrid[cursor.x, cursor.y].R = randomDoorIndex;
                    majorLabirynthGrid[w.x, w.y].L = randomDoorIndex;

                        randomDoorIndex = randomNumbersGenerator.GetRandomNumber(3, MinorLabirynthDimmension - 3);
                        if (randomDoorIndex % 2 == 0) randomDoorIndex++;

                    majorLabirynthGrid[neighboursForRandomSelect[randomNeighbour].position.x, neighboursForRandomSelect[randomNeighbour].position.y].L = randomDoorIndex;
                    majorLabirynthGrid[w.x, w.y].R = randomDoorIndex;
                    break;
                case 1:
                    //left
                    majorLabirynthGrid[cursor.x, cursor.y].L = randomDoorIndex;
                    majorLabirynthGrid[w.x, w.y].R = randomDoorIndex;

                    randomDoorIndex = randomNumbersGenerator.GetRandomNumber(3, MinorLabirynthDimmension - 3);
                    if (randomDoorIndex % 2 == 0) randomDoorIndex++;

                    majorLabirynthGrid[neighboursForRandomSelect[randomNeighbour].position.x, neighboursForRandomSelect[randomNeighbour].position.y].R = randomDoorIndex;
                    majorLabirynthGrid[w.x, w.y].L = randomDoorIndex;
                    break;
                case 2:
                    //top
                    majorLabirynthGrid[cursor.x, cursor.y].T = randomDoorIndex;
                    majorLabirynthGrid[w.x, w.y].B = randomDoorIndex;

                    randomDoorIndex = randomNumbersGenerator.GetRandomNumber(3, MinorLabirynthDimmension - 3);
                    if (randomDoorIndex % 2 == 0) randomDoorIndex++;

                    majorLabirynthGrid[neighboursForRandomSelect[randomNeighbour].position.x, neighboursForRandomSelect[randomNeighbour].position.y].B = randomDoorIndex;
                    majorLabirynthGrid[w.x, w.y].T = randomDoorIndex;
                    break;
                case 3:
                    //bottom
                    majorLabirynthGrid[cursor.x, cursor.y].B = randomDoorIndex;
                    majorLabirynthGrid[w.x, w.y].T = randomDoorIndex;

                    randomDoorIndex = randomNumbersGenerator.GetRandomNumber(3, MinorLabirynthDimmension - 3);
                    if (randomDoorIndex % 2 == 0) randomDoorIndex++;

                    majorLabirynthGrid[neighboursForRandomSelect[randomNeighbour].position.x, neighboursForRandomSelect[randomNeighbour].position.y].T = randomDoorIndex;
                    majorLabirynthGrid[w.x, w.y].B = randomDoorIndex;
                    break;
            }

            //"timeout" variable decrising
            c--;

        }


        Debug.Log("major labirynth generating done!");

        //change spawning flag after generating done
        generatingDone = true;
    }

    //method to find and return neighbours of object in particular location
    private List<MajorCell> GetNeighbours(IntVector2 location)
    {
        List<MajorCell> neighbours = new List<MajorCell>();

        IntVector2 dir = new IntVector2();

        //iterating via every direction
        for (int i = 0; i < 4; i++)
        {
            //preparing direction variable for each direction
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


            //skip if direction pointing outside grid or pointed element isn`t EMPTY type
            if (dir.x < 0 || dir.x >= MajorLabirynthDimmension || dir.y < 0 || dir.y >= MajorLabirynthDimmension)
            {
                neighbours.Add(null);
                continue;
            }
            if (majorLabirynthGrid[dir.x, dir.y].type == MajorCell.CELL_TYPE.WALL || majorLabirynthGrid[dir.x, dir.y].type == MajorCell.CELL_TYPE.OBSTICLE || majorLabirynthGrid[dir.x, dir.y].type == MajorCell.CELL_TYPE.PATH)
            {
                neighbours.Add(null);
                continue;
            }

            //add neighbour to list
            neighbours.Add(majorLabirynthGrid[dir.x, dir.y]);
        }

        return neighbours;

    }

    List<GameObject> objectsToInicialize = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        if(objectsToInicialize.Count > 0)
        {
            if (objectsToInicialize[0].GetComponent<MajorCellObject>().spawnDone)
            {
                if(objectsToInicialize.Count > 1)
                {
                    objectsToInicialize[1].GetComponent<MajorCellObject>().Initialize(MinorLabirynthDimmension, MinorSize, MajorSize);
                }
                
                objectsToInicialize.RemoveAt(0);
            }
        }
        
        //wait with spawning to generator finish generating and execute Spawn() method once
        if (generatingDone)
        {
            generatingDone = false;
            Spawn();
            objectsToInicialize[0].GetComponent<MajorCellObject>().Initialize(MinorLabirynthDimmension, MinorSize, MajorSize);
        }
    }

    //spawning gameObjects of labirynth elements
    private void Spawn()
    {
        for (int y = 0; y < MajorLabirynthDimmension; y++)
        {
            for (int x = 0; x < MajorLabirynthDimmension; x++)
            {
                Vector3 position = new Vector3(transform.position.x - (globalSize / 2) + (x * MajorSize) + (MajorSize / 2), transform.position.y - (globalSize / 2) + (y * MajorSize) + (MajorSize / 2), 0);

                majorCellObjectsGrid[x, y] = (GameObject)Instantiate(majorCellPrefab, position, Quaternion.Euler(0, 0, 0));
                majorCellObjectsGrid[x, y].transform.localScale = new Vector3(MajorSize, MajorSize, 0);
                majorCellObjectsGrid[x, y].transform.parent = transform;
                majorCellObjectsGrid[x, y].GetComponent<MajorCellObject>().type = majorLabirynthGrid[x, y].type;
                majorCellObjectsGrid[x, y].GetComponent<MajorCellObject>().L = majorLabirynthGrid[x, y].L;
                majorCellObjectsGrid[x, y].GetComponent<MajorCellObject>().T = majorLabirynthGrid[x, y].T;
                majorCellObjectsGrid[x, y].GetComponent<MajorCellObject>().R = majorLabirynthGrid[x, y].R;
                majorCellObjectsGrid[x, y].GetComponent<MajorCellObject>().B = majorLabirynthGrid[x, y].B;

                

                objectsToInicialize.Add(majorCellObjectsGrid[x, y]);

                //majorCellObjectsGrid[x, y].GetComponent<MajorCellObject>().Initialize(MinorLabirynthDimmension, MinorSize, MajorSize, majorLabirynthGrid[x, y].type, L, T, R, B);

            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        float offset = 0.4f;
        Gizmos.DrawWireCube(transform.position/* - new Vector3(globalSize / 2, globalSize / 2, 0)*/, new Vector3(globalSize * transform.localScale.x + offset, globalSize * transform.localScale.y + offset, 0));
    }
}
