using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LabirynthGenerator
{
    public enum GENERATOR {STANDARD };  //labirynth generator type; STANDARD - basic mode

    GENERATOR generatorType;    //variable to hold generator type

    public int width, height;  //count of cells in horizontal and vertical

    public float cellSize = 1;

    public LabirynthCell[,] labirynthGrid;

    Vector2 cursor;

    public bool done = false;

    //constructors
    //////////////////////////////////////////////////////////////////////
    public LabirynthGenerator(int _width, int _height, Vector2 cursorStart)
    {
        width = _width;
        height = _height;
        generatorType = GENERATOR.STANDARD;
        labirynthGrid = new LabirynthCell[width, height];
        cursor = cursorStart;
    }

    public LabirynthGenerator(int _width, int _height, Vector2 cursorStart, GENERATOR _generatorMod)
    {
        width = _width;
        height = _height;
        generatorType = _generatorMod;
        labirynthGrid = new LabirynthCell[width, height];
        cursor = cursorStart;
    }
    //////////////////////////////////////////////////////////////////////

    public void Generate()
    {
        switch (generatorType)
        {
            case GENERATOR.STANDARD:
                cellSize = 3;
                break;
        }

        Thread myThread = new Thread(new ThreadStart(Generating));
        myThread.Start();       //executing generating thread
    }

    private void Generating()
    {
        if(labirynthGrid != null)
        {
            //filling grid with LabirynthCell objects
            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    labirynthGrid[x, y] = new LabirynthCell(x, y);
                    Debug.Log("cell created");
                }
            }

            done = true;
        }
        else
        {
            Debug.LogError("There is no labirynthGrid object");
        }
    }

    

    
}
