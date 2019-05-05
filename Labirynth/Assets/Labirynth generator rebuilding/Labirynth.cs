using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;



public class Labirynth : MonoBehaviour
{
    MinorGrid[ , ] grid;

    int majorDimension, minorDimension;

    IntVector2 cursor;

    [SerializeField]
    RandomNumbersGenerator randomNumbersGenerator;

    [SerializeField]
    [Range(0, 100)]
    int majorRepeatChance;

    [SerializeField]
    [Range(0, 100)]
    int minorRepeatChance;

    // Start is called before the first frame update
    void Start()
    {
        GenerateLabirynth(5, 5, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateLabirynth(int _majorDimension, int _minorDimension, float _minorSize)
    {
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

        Thread generator = new Thread(Generating);
        generator.Start();

        
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

        int majorTimeout = (int)Mathf.Pow(majorDimension, 2) * 2;
        Debug.Log("major timeout control: " + majorTimeout);
        while(walkedMajorCells.Count > 0 && majorTimeout > 0)
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

            IntVector2 w = new IntVector2((cursor.x + neighboursForRandomSelect[randomNeighbour].position.x) / 2, (cursor.y + neighboursForRandomSelect[randomNeighbour].position.y) / 2);
            grid[w.x, w.y].type = LabirynthCell.TYPE.PATH;
            minorLabirynthsToGenerate.Add(grid[w.x, w.y]);



            for(int i = 0; i < minorLabirynthsToGenerate.Count; i++)
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

        LabirynthCell[ , ] allCells = new LabirynthCell[majorDimension * minorDimension, majorDimension * minorDimension];

        for(int y = 0; y < majorDimension * minorDimension; y++)
        {
            for (int x = 0; x < majorDimension * minorDimension; x++)
            {

                int majorX = x / minorDimension;
                int majorY = y / minorDimension;
                int minorX = x % minorDimension;
                int minorY = y % minorDimension;
                Debug.Log("(" + majorX + ", " + majorY + ") (" + minorX + ", " + minorY + ") " + grid[majorX, majorY].minorGrid);
                allCells[x, y] = grid[majorX, majorY].minorGrid[minorX, minorY];
            }
        }

        String d = "";
        for (int y = 0; y < majorDimension * minorDimension; y++)
        {
            for (int x = 0; x < majorDimension * minorDimension; x++)
            {
                switch(allCells[x, y].type)
                {
                    case LabirynthCell.TYPE.WALL:
                        d += "#";
                        break;
                    case LabirynthCell.TYPE.PATH:
                        d += "@";
                        break;
                    default:
                        d += "x";
                        break;
                }
                
                d += " ";
            }
            Debug.Log(d);
            d = "";
        }

        

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

    }
}
