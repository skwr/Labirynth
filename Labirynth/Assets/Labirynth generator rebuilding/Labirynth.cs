using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;



public class Labirynth : MonoBehaviour, ITrigger
{
    Thread generator;

    MinorGrid[ , ] grid;

    [SerializeField]
    int majorDimension = 5, minorDimension = 5;

    [SerializeField]
    float minorSize = 1;

    IntVector2 cursor;

    RandomNumbersGenerator randomNumbersGenerator;

    [SerializeField]
    [Range(0, 100)]
    int majorRepeatChance;

    [SerializeField]
    [Range(0, 100)]
    int minorRepeatChance;

    [SerializeField]
    GameObject wallPrefab, pathPrefab;

    [SerializeField]
    Texture2D tx1, tx2;

    bool generatingDone = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Execute()
    {
        GenerateLabirynth();
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

    public void GenerateLabirynth()
    {
        GenerateLabirynth(majorDimension, minorDimension, minorSize);
    }

    public void GenerateLabirynth(int _majorDimension, int _minorDimension, float _minorSize)
    {
        majorDimension = _majorDimension;
        minorDimension = _minorDimension;
        minorSize = _minorSize;

        if(randomNumbersGenerator == null)
        {
            GameObject rndObject = GameObject.FindGameObjectWithTag("RandomNumbersGenerator");
            if(rndObject != null)
            {
                randomNumbersGenerator = rndObject.GetComponent<RandomNumbersGenerator>();
                if(randomNumbersGenerator == null)
                {
                    Debug.LogWarning("there is no RandomNumbersGenerator attached to found object");
                    return;
                }
            }
            else
            {
                Debug.LogWarning("can find RandomNumbersGenerator instance");
                return;
            }
        }
        
        

        cursor = new IntVector2(1, 1);

        majorDimension = _majorDimension;
        minorDimension = _minorDimension;

        generator = new Thread(Generating);
        generator.Start();

        
    }

    private void OnApplicationQuit()
    {
        
        if (generator.IsAlive)
        {
            Debug.Log("aborting generator thread");
            generator.Abort();
        }
        
    }

    private void Generating()
    {
        Debug.Log("thread begin");

        grid = new MinorGrid[majorDimension, majorDimension];

        for (int y = 0; y < majorDimension; y++)
        {
            for (int x = 0; x < majorDimension; x++)
            {
                if(x%2 != 0 && y%2 != 0)
                {
                    grid[x, y] = new MinorGrid(new IntVector2(x, y), minorDimension, LabirynthCell.TYPE.WALKABLE, randomNumbersGenerator, minorRepeatChance);
                }
                else
                {
                    grid[x, y] = new MinorGrid(new IntVector2(x, y), minorDimension, LabirynthCell.TYPE.WALL, randomNumbersGenerator, minorRepeatChance);
                }
                
            }
        }

        

        List<MinorGrid> walkedMajorCells = new List<MinorGrid>();
        grid[cursor.x, cursor.y].type = LabirynthCell.TYPE.PATH;
        walkedMajorCells.Add(grid[cursor.x, cursor.y]);

        grid[cursor.x, cursor.y].Generate();

        int majorTimeout = 5;// (int)Mathf.Pow(majorDimension, 2) * 2;
        //Debug.Log("major timeout control: " + majorTimeout);

        
        while (walkedMajorCells.Count > 0 && majorTimeout > 0)
        {
            
            int repeat = randomNumbersGenerator.GetRandomNumber(0, 101);     //get random number to draw if generator should make next step from last cursor position or draw new position from walkedCells
            int randomWalked;
            if (repeat < majorRepeatChance)
            {
                randomWalked = walkedMajorCells.Count - 1;
                cursor = walkedMajorCells[randomWalked].position;
            }
            else
            {
                randomWalked = randomNumbersGenerator.GetRandomNumber(0, walkedMajorCells.Count);
                cursor = walkedMajorCells[randomWalked].position;
            }
            

            //get all neighbours of spot that is pointed by curosr
            List<MinorGrid> neighbours = GetNeighbours(cursor);

            List<MinorGrid> neighboursForRandomSelect = new List<MinorGrid>();
            for (int i = 0; i < neighbours.Count; i++)
            {
                if (neighbours[i] == null) continue;
                neighboursForRandomSelect.Add(neighbours[i]);
            }

            

            if (neighboursForRandomSelect.Count <= 0)
            {
                walkedMajorCells.RemoveAt(randomWalked);
                continue;
            }

            int randomNeighbour = randomNumbersGenerator.GetRandomNumber(0, neighboursForRandomSelect.Count);

            List<MinorGrid> minorLabirynthsToGenerate = new List<MinorGrid>();

            walkedMajorCells.Add(neighboursForRandomSelect[randomNeighbour]);
            grid[neighboursForRandomSelect[randomNeighbour].position.x, neighboursForRandomSelect[randomNeighbour].position.y].type = LabirynthCell.TYPE.PATH;
            minorLabirynthsToGenerate.Add(grid[neighboursForRandomSelect[randomNeighbour].position.x, neighboursForRandomSelect[randomNeighbour].position.y]);



            for (int i = 0; i < 4; i++)
            {
                if (neighboursForRandomSelect[randomNeighbour].Equals(neighbours[i]))
                {
                    switch (i)
                    {
                        case 0:
                            //right
                            Debug.Log("right");
                            break;
                        case 1:
                            //left
                            Debug.Log("left");
                            break;
                        case 2:
                            //up
                            Debug.Log("up");
                            break;
                        case 3:
                            //down
                            Debug.Log("down");
                            break;
                    }
                }
            }

            IntVector2 w = new IntVector2((cursor.x + neighboursForRandomSelect[randomNeighbour].position.x) / 2, (cursor.y + neighboursForRandomSelect[randomNeighbour].position.y) / 2);
            grid[w.x, w.y].type = LabirynthCell.TYPE.PATH;
            minorLabirynthsToGenerate.Add(grid[w.x, w.y]);

            

            for (int i = 0; i < minorLabirynthsToGenerate.Count; i++)
            {
                minorLabirynthsToGenerate[i].Generate();
            }

            



            majorTimeout--;

            if(majorTimeout <= 0)
            {
                Debug.LogWarning("MAJOR GENERATING TIMEOUT");
                return;
            }

            
        }




        Debug.Log("tester");


        generatingDone = true;

        Debug.Log("generated");
    }


    private List<MinorGrid> GetNeighbours(IntVector2 location)
    {
        List<MinorGrid> neighbours = new List<MinorGrid>();

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
            if (dir.x < 0 || dir.x >= majorDimension || dir.y < 0 || dir.y >= majorDimension)
            {
                neighbours.Add(null);
                continue;
            }
            if (grid[dir.x, dir.y].type == LabirynthCell.TYPE.WALL || grid[dir.x, dir.y].type == LabirynthCell.TYPE.PATH)
            {
                neighbours.Add(null);
                continue;
            }

            //add neighbour to list
            neighbours.Add(grid[dir.x, dir.y]);
        }

        

        return neighbours;

    }


    public void Spawn()
    {
        for (int y1 = 0; y1 < majorDimension; y1++)
        {
            for (int x1 = 0; x1 < majorDimension; x1++)
            {
                for (int y2 = 0; y2 < minorDimension; y2++)
                {
                    for (int x2 = 0; x2 < minorDimension; x2++)
                    {
                        if(grid[x1, y1].minorGrid[x2, y2].type == LabirynthCell.TYPE.EMPTY)
                        {
                            continue;
                        }
                        
                        if (grid[x1, y1].minorGrid[x2, y2].type == LabirynthCell.TYPE.PATH)
                        {
                            GameObject cell = (GameObject)Instantiate(pathPrefab, transform);
                            cell.transform.localPosition = new Vector3(x1 * minorDimension * minorSize + x2 * minorSize, y1 * minorDimension * minorSize + y2 * minorSize, 0);

                        }
                        else
                        {
                            GameObject cell = (GameObject)Instantiate(wallPrefab, transform);
                            cell.transform.localPosition = new Vector3(x1 * minorDimension * minorSize + x2 * minorSize, y1 * minorDimension * minorSize + y2 * minorSize, 0);

                        }

                    }
                }
                
            }
        }
    }
}
