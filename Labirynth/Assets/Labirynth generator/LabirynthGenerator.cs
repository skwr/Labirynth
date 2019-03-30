using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class myEvent : UnityEvent<int[,]>
{

}

public class LabirynthGenerator
{
    public enum GENERATOR {STANDARD };  //labirynth generator type; STANDARD - basic mode

    GENERATOR generatorType;    //variable to hold generator type

    public int width, height;  //count of cells in horizontal and vertical

    public float cellSize = 1;

    

    Vector2 cursor;

    

    

    myEvent generatingDone;

    GameObject parent;
    GameMaster gm;

    //constructors
    //////////////////////////////////////////////////////////////////////
    public LabirynthGenerator(int _width, int _height, Vector2 cursorStart, GameObject _parent)
    {
        width = _width;
        height = _height;
        generatorType = GENERATOR.STANDARD;
        cursor = cursorStart;
        parent = _parent;
        gm = parent.GetComponent<GameMaster>();
    }

    public LabirynthGenerator(int _width, int _height, Vector2 cursorStart, GENERATOR _generatorMod, GameObject _parent)
    {
        width = _width;
        height = _height;
        generatorType = _generatorMod;
        cursor = cursorStart;
        parent = _parent;
        gm = parent.GetComponent<GameMaster>();
    }
    //////////////////////////////////////////////////////////////////////

    public void Generate()
    {
        //to do:
        //configure different generator types for creating different levels of labirynth and/or different level of complexity
        //
        switch (generatorType)
        {
            case GENERATOR.STANDARD:
                cellSize = 1;
                break;
        }

        Thread myThread = new Thread(new ThreadStart(Generating));
        myThread.Start();       //executing generating thread
    }

    //generating thread
    private void Generating()
    {
        Debug.Log("generating...");

        int[,] finalLabitynthMatrix = new int[width, height];       //final matrix to send via event

        LabirynthCell[,] labirynthGrid = new LabirynthCell[width, height];  //temporary matrix to operate on


        if (labirynthGrid != null)
        {
            //filling grid with empty LabirynthCell objects
            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    labirynthGrid[x, y] = new LabirynthCell(x, y);
                    Debug.Log("cell created");
                }
            }

            
        }
        else
        {
            Debug.LogError("There is no labirynthGrid object");
        }

        //to do:
        //here insert generator code
        //

        //filling final matrix with cell statuses
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                
                finalLabitynthMatrix[x, y] = (int)labirynthGrid[x, y].cellStatus;
                

                Debug.Log("cell created");
            }
        }

        //sending event after generating done
        generatingDone = new myEvent();
        generatingDone.AddListener(gm.eventTest);
        generatingDone.Invoke(finalLabitynthMatrix);
    }

    

    
}
