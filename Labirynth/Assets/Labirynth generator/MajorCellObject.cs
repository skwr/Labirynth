using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MajorCellObject : RebuildCellObject
{
    GameObject[,] minorCellObjectsGrid;
    MajorCell[,] minorLabirynthGrid;

    [SerializeField]
    GameObject minorCellPrefab = null;

    [SerializeField]
    SpriteRenderer thisRenderer = null;

    [SerializeField]
    List<Sprite> sprites = null;

    [SerializeField]
    RandomNumbersGenerator randomNumbersGenerator;

    [SerializeField]
    [Range(0, 100)]
    int repeatChance = 50;  //chance for generator to make step from current cursor location or random from walked positions

    bool generatingDone = false;    //flag to let this object spawn generated labirynth


    float minorSize;    //size of minor object in space, getted from labirynth object
    int minorDimension; //dimension of minor labirynth, getted form labirynth object

    float majorSize;    //size of major object in space, used to spawn minor objects in right position

    IntVector2 cursor;  //variable to hold position of current generator step

    

    public void Initialize(int MinorDimension, float MinorSize, float MajorSize, MajorCell.CELL_TYPE type)
    {
        //initialize this object generator
        //creating its own labirynth

        
        minorSize = MinorSize;
        minorDimension = MinorDimension;
        majorSize = MajorSize;
        

        size = new Vector3(MajorSize, MajorSize, 0);

        //getting instance of global random numbers generator
        randomNumbersGenerator = GameObject.FindGameObjectWithTag("RandomNumbersGenerator").GetComponent<RandomNumbersGenerator>();

        //do specific things depends on this object type
        switch (type)
        {
            case MajorCell.CELL_TYPE.EMPTY:

                thisRenderer.sprite = sprites[0];
                break;
            case MajorCell.CELL_TYPE.OBSTICLE:
                thisRenderer.sprite = sprites[0];
                break;
            case MajorCell.CELL_TYPE.PATH:
                //start generating minor labirynth
                minorLabirynthGrid = new MajorCell[minorDimension, minorDimension];
                Thread generator = new Thread(Generating);
                generator.Start();
                break;
            case MajorCell.CELL_TYPE.WALL:
                thisRenderer.sprite = sprites[0];
                break;
        }

        

        
    }

    //method executed on thread
    private void Generating()
    {

        //filling grid width basic objects
        //place spots that will be always walked (EMPTY for beggining), spots that will be always wall (OBSTICLE) and spots that will be changable (WALL)
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

        //seting cursor start position
        //that isn`t realy matter where generator start, but it has to be odd number
        cursor = new IntVector2(1, 1);

        //preparing list of walked cells
        //generator is choosing one of this elements to make next step
        List<MajorCell> walkedCells = new List<MajorCell>();

        //inserting first element to list, it is that element what is cursor pointing at
        minorLabirynthGrid[cursor.x, cursor.y].type = MajorCell.CELL_TYPE.PATH;
        walkedCells.Add(minorLabirynthGrid[cursor.x, cursor.y]);
        
        //generator starting

        int c = 1000;   //"timeout" variable, in case that something go wrong
        while (walkedCells.Count > 0 && c > 0)  //repeat until there isn`t any walked spot with neightbours walkable
        {
            int repeat = randomNumbersGenerator.GetRandomNumber(0, 101);    //get random number to draw if generator should make next step from last cursor position or draw new position from walkedCells
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


            //remove this spot form walked if it haven`t got any neighbour
            if (neighbours.Count <= 0)
            {
                walkedCells.RemoveAt(randomWalked);
                continue;
            }

            //get random neighbour index from list
            int randomNeighbour = randomNumbersGenerator.GetRandomNumber(0, neighbours.Count);

            //inserting selected neighbour int
            walkedCells.Add(neighbours[randomNeighbour]);
            minorLabirynthGrid[neighbours[randomNeighbour].position.x, neighbours[randomNeighbour].position.y].type = MajorCell.CELL_TYPE.PATH;

            //changing object omitted on generator step
            IntVector2 w = new IntVector2((cursor.x + neighbours[randomNeighbour].position.x) / 2, (cursor.y + neighbours[randomNeighbour].position.y) / 2);
            minorLabirynthGrid[w.x, w.y].type = MajorCell.CELL_TYPE.PATH;

            //"timeout" variable decrising
            c--;

        }


        Debug.Log("minor labirynth generating done!");

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
            if (dir.x < 0 || dir.x >= minorDimension || dir.y < 0 || dir.y >= minorDimension) continue;
            if (minorLabirynthGrid[dir.x, dir.y].type == MajorCell.CELL_TYPE.WALL || minorLabirynthGrid[dir.x, dir.y].type == MajorCell.CELL_TYPE.OBSTICLE || minorLabirynthGrid[dir.x, dir.y].type == MajorCell.CELL_TYPE.PATH) continue;

            //add neighbour to list
            neighbours.Add(minorLabirynthGrid[dir.x, dir.y]);
        }

        return neighbours;

    }

    void Update()
    {
        //wait with spawning to generator finish generating and execute Spawn() method once
        if (generatingDone)
        {
            Spawn();
            generatingDone = false;
        }
    }

    //spawning gameObjects of labirynth elements
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
