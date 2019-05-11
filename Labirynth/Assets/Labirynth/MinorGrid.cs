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

    IntVector2 cursor;

    public MinorGrid(IntVector2 _position, int _minorDimension, LabirynthCell.TYPE _type, RandomNumbersGenerator _randomNumbersGenerator, int _minorRepeatChance)
    {
        //setting up properties
        position = _position;
        minorDimension = _minorDimension;
        type = _type;
        randomNumbersGenerator = _randomNumbersGenerator;
        minorRepeatChance = _minorRepeatChance;
        minorGrid = new LabirynthCell[minorDimension, minorDimension];
        cursor = new IntVector2(1, 1);

        //filling grid with empty cells (for objects that aren`t generated - majorWalls)
        for (int y = 0; y < minorDimension; y++)
        {
            for (int x = 0; x < minorDimension; x++)
            {
                minorGrid[x, y] = new LabirynthCell(new IntVector2(x, y), LabirynthCell.TYPE.EMPTY);
            }
        }
    }

    public void Generate()
    {
        //filling grid with base pattern
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

        

        List<LabirynthCell> walkedMinorCells = new List<LabirynthCell>();

        //set first cell as walked
        minorGrid[cursor.x, cursor.y].type = LabirynthCell.TYPE.PATH;
        walkedMinorCells.Add(minorGrid[cursor.x, cursor.y]);


        int minorTimeout = (int)Mathf.Pow(minorDimension, 2) * 2;//timeout variable to stop while loop if something goes wrong
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

            //get all neighbours of spot that is pointed by curosr
            List<LabirynthCell> neighbours = GetNeighbours(cursor);

            //remove from walked cells if haven`t got any walkable neighbour
            if (neighbours.Count <= 0)
            {
                walkedMinorCells.RemoveAt(randomWalked);
                continue;
            }

            //draw neighbour to next step
            int randomNeighbour = randomNumbersGenerator.GetRandomNumber(0, neighbours.Count);
            LabirynthCell selectedNeighbour = neighbours[randomNeighbour];

            //setting neighbour as walked and add it to walked cells
            walkedMinorCells.Add(selectedNeighbour);
            minorGrid[selectedNeighbour.position.x, selectedNeighbour.position.y].type = LabirynthCell.TYPE.PATH;

            //calculate position of cell between current cell and selected neighbour and setting between cell as walked
            IntVector2 w = new IntVector2((cursor.x + selectedNeighbour.position.x) / 2, (cursor.y + selectedNeighbour.position.y) / 2);
            minorGrid[w.x, w.y].type = LabirynthCell.TYPE.PATH;

            //decrement timeout variable
            minorTimeout--;

            if (minorTimeout <= 0)
            {
                Debug.LogWarning("MINOR GENERATING TIMEOUT");
                return;
            }
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


            //skip if direction pointing outside grid or pointed element isn`t WALKABLE type
            if (dir.x < 0 || dir.x >= minorDimension || dir.y < 0 || dir.y >= minorDimension) continue;
            if (minorGrid[dir.x, dir.y].type != LabirynthCell.TYPE.WALKABLE) continue;

            //add neighbour to list
            neighbours.Add(minorGrid[dir.x, dir.y]);
        }
        return neighbours;
    }
}
