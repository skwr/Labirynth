using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class ToSpawnerEvent : UnityEvent<MasterLabirynthCell>
{

}

public class MasterLabirynthGenerator
{
    LabirynthSpawner parentSpawner;

    MasterLabirynthCell[,] labirynthGrid;

    IntVector2 size;



    public MasterLabirynthGenerator(IntVector2 _size, LabirynthSpawner _parentSpawner)
    {
        size = _size;

        parentSpawner = _parentSpawner;
    }

    public void Generate()
    {
        labirynthGrid = new MasterLabirynthCell[size.x, size.y];    //initialize grid

        Thread gen = new Thread(GeneratingThread);
        gen.Start();
        
    }

    private void GeneratingThread()
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                labirynthGrid[x, y] = new MasterLabirynthCell(new IntVector2(x, y), LabirynthCell.CELL_TYPE.WALL);
            }
        }

        

        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                ToSpawnerEvent spawnEvent = new ToSpawnerEvent();
                spawnEvent.AddListener(parentSpawner.SpawnLabirynthObject);
                spawnEvent.Invoke(labirynthGrid[x, y]);

                
            }
        }

        //wstawic oczekiwanie na event zakonczenia spawnowania
        int c = 100;
        while (parentSpawner.spawnQueue.Count > 0 && c > 0)
        {
            c--;
        }

        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if(x%2 != 0 && y%2 != 0)
                {
                    ToSpawnerEvent updateEvent = new ToSpawnerEvent();
                    updateEvent.AddListener(parentSpawner.UpdateLabirynthObject);
                    updateEvent.Invoke(labirynthGrid[x, y]);
                }
                


            }
        }


        Debug.Log("generating thread done");


    }
    
}
