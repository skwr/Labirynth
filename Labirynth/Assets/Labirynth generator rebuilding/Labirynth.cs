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
    GameObject wallPrefab, pathPrefab, majorWallPrefab;

    

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
        if (generator != null)
        {
            if (generator.IsAlive)
            {
                Debug.Log("aborting generator thread");
                generator.Abort();
            }
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

        int majorTimeout = (int)Mathf.Pow(majorDimension, 2) * 2;
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

            IntVector2 stepDirection = new IntVector2(neighboursForRandomSelect[randomNeighbour].position.x - cursor.x, neighboursForRandomSelect[randomNeighbour].position.y - cursor.y);

            List<MinorGrid> minorLabirynthsToGenerate = new List<MinorGrid>();

            walkedMajorCells.Add(neighboursForRandomSelect[randomNeighbour]);
            grid[neighboursForRandomSelect[randomNeighbour].position.x, neighboursForRandomSelect[randomNeighbour].position.y].type = LabirynthCell.TYPE.PATH;
            minorLabirynthsToGenerate.Add(grid[neighboursForRandomSelect[randomNeighbour].position.x, neighboursForRandomSelect[randomNeighbour].position.y]);


            
            

            IntVector2 w = new IntVector2((cursor.x + neighboursForRandomSelect[randomNeighbour].position.x) / 2, (cursor.y + neighboursForRandomSelect[randomNeighbour].position.y) / 2);
            grid[w.x, w.y].type = LabirynthCell.TYPE.PATH;
            minorLabirynthsToGenerate.Add(grid[w.x, w.y]);

            

            for (int i = 0; i < minorLabirynthsToGenerate.Count; i++)
            {
                minorLabirynthsToGenerate[i].Generate();
            }

            int firstDoor;
            int secondDoor;

            firstDoor = randomNumbersGenerator.GetRandomNumber(1, minorDimension - 2);
            if(firstDoor%2 == 0)
            {
                if(firstDoor + 1 <= minorDimension - 2)
                {
                    firstDoor += 1;
                }
                else if(firstDoor - 1 >= 1)
                {
                    firstDoor -= 1;
                }
            }


            secondDoor = randomNumbersGenerator.GetRandomNumber(1, minorDimension - 2);

            if (secondDoor % 2 == 0)
            {
                if (secondDoor + 1 <= minorDimension - 2)
                {
                    secondDoor += 1;
                }
                else if (secondDoor - 1 >= 1)
                {
                    secondDoor -= 1;
                }
            }

            if (stepDirection.x != 0)
            {
                if (stepDirection.x < 0)
                {
                    grid[neighboursForRandomSelect[randomNeighbour].position.x, neighboursForRandomSelect[randomNeighbour].position.y].minorGrid[minorDimension - 1, secondDoor].type = LabirynthCell.TYPE.PATH;

                    grid[cursor.x, cursor.y].minorGrid[0, firstDoor].type = LabirynthCell.TYPE.PATH;

                    grid[w.x, w.y].minorGrid[0, secondDoor].type = LabirynthCell.TYPE.PATH;
                    grid[w.x, w.y].minorGrid[minorDimension - 1, firstDoor].type = LabirynthCell.TYPE.PATH;
                }
                else if (stepDirection.x > 0)
                {
                    grid[neighboursForRandomSelect[randomNeighbour].position.x, neighboursForRandomSelect[randomNeighbour].position.y].minorGrid[0, secondDoor].type = LabirynthCell.TYPE.PATH;

                    grid[cursor.x, cursor.y].minorGrid[minorDimension - 1, firstDoor].type = LabirynthCell.TYPE.PATH;

                    grid[w.x, w.y].minorGrid[0, firstDoor].type = LabirynthCell.TYPE.PATH;
                    grid[w.x, w.y].minorGrid[minorDimension - 1, secondDoor].type = LabirynthCell.TYPE.PATH;
                }

                
            }
            else if (stepDirection.y != 0)
            {
                if (stepDirection.y < 0)
                {
                    grid[neighboursForRandomSelect[randomNeighbour].position.x, neighboursForRandomSelect[randomNeighbour].position.y].minorGrid[secondDoor, minorDimension - 1].type = LabirynthCell.TYPE.PATH;

                    grid[cursor.x, cursor.y].minorGrid[firstDoor, 0].type = LabirynthCell.TYPE.PATH;

                    grid[w.x, w.y].minorGrid[secondDoor, 0].type = LabirynthCell.TYPE.PATH;
                    grid[w.x, w.y].minorGrid[firstDoor, minorDimension - 1].type = LabirynthCell.TYPE.PATH;
                }
                else if (stepDirection.y > 0)
                {
                    grid[neighboursForRandomSelect[randomNeighbour].position.x, neighboursForRandomSelect[randomNeighbour].position.y].minorGrid[secondDoor, 0].type = LabirynthCell.TYPE.PATH;

                    grid[cursor.x, cursor.y].minorGrid[firstDoor, minorDimension - 1].type = LabirynthCell.TYPE.PATH;

                    grid[w.x, w.y].minorGrid[firstDoor, 0].type = LabirynthCell.TYPE.PATH;
                    grid[w.x, w.y].minorGrid[secondDoor, minorDimension - 1].type = LabirynthCell.TYPE.PATH;
                }

                
            }



            majorTimeout--;

            if(majorTimeout <= 0)
            {
                Debug.LogWarning("MAJOR GENERATING TIMEOUT");
                return;
            }

            
        }

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
                if(grid[x1, y1].type == LabirynthCell.TYPE.WALL)
                {
                    GameObject majorWall = Instantiate(majorWallPrefab, transform);
                    majorWall.transform.localPosition = new Vector3(x1 * minorDimension * minorSize + ((minorDimension * minorSize) / 2) - (minorSize / 2), y1 * minorDimension * minorSize + ((minorDimension * minorSize) / 2) - (minorSize / 2), 0);
                    majorWall.transform.localScale = new Vector3(minorDimension * minorSize, minorDimension * minorSize, 1);
                    MaterialPropertyBlock mpb = new MaterialPropertyBlock();
                    majorWall.GetComponent<SpriteRenderer>().GetPropertyBlock(mpb);
                    mpb.SetFloat("Vector1_A3318CCE", minorDimension);
                    majorWall.GetComponent<SpriteRenderer>().SetPropertyBlock(mpb);
                }
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
                            cell.transform.localScale = new Vector3(minorSize, minorSize, 1);
                        }
                        else
                        {
                            GameObject cell = (GameObject)Instantiate(wallPrefab, transform);
                            cell.transform.localPosition = new Vector3(x1 * minorDimension * minorSize + x2 * minorSize, y1 * minorDimension * minorSize + y2 * minorSize, 0);
                            cell.transform.localScale = new Vector3(minorSize, minorSize, 1);
                        }

                    }
                }
                
            }
        }
    }
}
