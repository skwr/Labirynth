using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinorGrid
{
    public LabirynthCell.TYPE type;

    public LabirynthCell[,] minorGrid;

    int minorDimension;

    public IntVector2 position;

    RandomNumbersGenerator randomNumbersGenerator;

    int minorRepeatChance;

    public MinorGrid(IntVector2 _position, int _minorDimension, LabirynthCell.TYPE _type, RandomNumbersGenerator _randomNumbersGenerator, int _minorRepeatChance)
    {
        position = _position;
        minorDimension = _minorDimension;
        type = _type;
        randomNumbersGenerator = _randomNumbersGenerator;

        minorRepeatChance = _minorRepeatChance;

        minorGrid = new LabirynthCell[minorDimension, minorDimension];

        

        for (int y = 0; y < minorDimension; y++)
        {
            for (int x = 0; x < minorDimension; x++)
            {
                
                minorGrid[x, y] = new LabirynthCell(new IntVector2(x, y), LabirynthCell.TYPE.EMPTY);
                

            }
        }


        //Debug.Log("minor grid prepared");
    }

    public void Generate()
    {
        //Debug.Log("generating minor labirynth");

        for (int y = 0; y < minorDimension; y++)
        {
            for (int x = 0; x < minorDimension; x++)
            {
                if (x % 2 != 0 && y % 2 != 0)
                {
                    minorGrid[x, y] = new LabirynthCell(new IntVector2(x, y), LabirynthCell.TYPE.WALKABLE);
                }
                else
                {
                    minorGrid[x, y] = new LabirynthCell(new IntVector2(x, y), LabirynthCell.TYPE.WALL);
                }

            }
        }

        IntVector2 cursor = new IntVector2(1, 1);

        List<LabirynthCell> walkedMinorCells = new List<LabirynthCell>();
        minorGrid[cursor.x, cursor.y].type = LabirynthCell.TYPE.PATH;
        walkedMinorCells.Add(minorGrid[cursor.x, cursor.y]);


        int minorTimeout = (int)Mathf.Pow(minorDimension, 2) * 2;
        //Debug.Log("minor timeout control: " + minorTimeout);
        while (walkedMinorCells.Count > 0 && minorTimeout > 0)
        {
            int repeat = randomNumbersGenerator.GetRandomNumber(0, 101);     //get random number to draw if generator should make next step from last cursor position or draw new position from walkedCells
            int randomWalked;
            if (repeat < minorRepeatChance)
            {
                randomWalked = walkedMinorCells.Count - 1;
                cursor = walkedMinorCells[randomWalked].position;
            }
            else
            {
                randomWalked = randomNumbersGenerator.GetRandomNumber(0, walkedMinorCells.Count);
                cursor = walkedMinorCells[randomWalked].position;
            }

            List<LabirynthCell> neighbours = GetNeighbours(cursor);

            List<LabirynthCell> neighboursForRandomSelect = new List<LabirynthCell>();
            for (int i = 0; i < neighbours.Count; i++)
            {
                if (neighbours[i] == null) continue;
                neighboursForRandomSelect.Add(neighbours[i]);
            }


            if (neighboursForRandomSelect.Count <= 0)
            {
                walkedMinorCells.RemoveAt(randomWalked);
                continue;
            }

            int randomNeighbour = randomNumbersGenerator.GetRandomNumber(0, neighboursForRandomSelect.Count);

            

            walkedMinorCells.Add(neighboursForRandomSelect[randomNeighbour]);
            minorGrid[neighboursForRandomSelect[randomNeighbour].position.x, neighboursForRandomSelect[randomNeighbour].position.y].type = LabirynthCell.TYPE.PATH;

            IntVector2 w = new IntVector2((cursor.x + neighboursForRandomSelect[randomNeighbour].position.x) / 2, (cursor.y + neighboursForRandomSelect[randomNeighbour].position.y) / 2);
            minorGrid[w.x, w.y].type = LabirynthCell.TYPE.PATH;

            minorTimeout--;
        }
    }

    private List<LabirynthCell> GetNeighbours(IntVector2 location)
    {
        List<LabirynthCell> neighbours = new List<LabirynthCell>();

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
            if (dir.x < 0 || dir.x >= minorDimension || dir.y < 0 || dir.y >= minorDimension)
            {
                neighbours.Add(null);
                continue;
            }
            if (minorGrid[dir.x, dir.y].type == LabirynthCell.TYPE.WALL || minorGrid[dir.x, dir.y].type == LabirynthCell.TYPE.PATH)
            {
                neighbours.Add(null);
                continue;
            }

            //add neighbour to list
            neighbours.Add(minorGrid[dir.x, dir.y]);
        }



        return neighbours;

    }



    public LabirynthCell GetCell(IntVector2 position)
    {
        return minorGrid[position.x, position.y];
    }

    public LabirynthCell.TYPE GetCellType(IntVector2 position)
    {
        return minorGrid[position.x, position.y].type;
    }
}
