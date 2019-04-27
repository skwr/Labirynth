using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using System;

public class ToSpawnerEvent : UnityEvent<MasterLabirynthCell>
{

}

public class MasterLabirynthGenerator
{
    LabirynthSpawner parentSpawner;

    MasterLabirynthCell[,] labirynthGrid;

    IntVector2 size;

    bool startUpdate = false;

    IntVector2 cursor;

    List<MasterLabirynthCell> walkedCells;

    int seed = 0;

    public MasterLabirynthGenerator(IntVector2 _size, LabirynthSpawner _parentSpawner)
    {
        size = _size;

        parentSpawner = _parentSpawner;
        cursor = new IntVector2((size.x - 1) / 2, (size.y - 1) / 2);
        walkedCells = new List<MasterLabirynthCell>();
    }

    public void Generate(int _seed)
    {
        seed = _seed;

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

        float counter = 0;
        int masterCounter = 1000;
        long t = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        while (!parentSpawner.breakAfterSpawn && masterCounter > 0)
        {
            if (!parentSpawner)
            {
                Debug.Log("spawner does not exist");
                return;
            }

            //dodac jakis timeout zeby nie zapieprzalo ciagle w razie klopotow!!!!!!!!!!!!!
            //to samo z update

            if (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond - t > 100)
            {
                

                counter += 0.1f;
                Debug.Log("waiting to spawn cells... " + counter + "s");
                masterCounter--;
                t = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            }
             
            
            
            
        }

        parentSpawner.breakAfterSpawn = false;
        

        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if(x%2 != 0 && y%2 != 0)
                {
                    UpdateCellObject(labirynthGrid[x, y], LabirynthCell.CELL_TYPE.EMPTY);
                    
                }
                


            }
        }

        masterCounter = 1000;
        counter = 0;
        t = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        while (!parentSpawner.breakAfterUpdate && masterCounter > 0)
        {

            if (!parentSpawner)
            {
                Debug.Log("spawner does not exist");
                return;
            }

            if (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond - t > 100)
            {
                counter += 0.1f;
                Debug.Log("waiting to update cells... " + counter + "s");
                t = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                masterCounter--;
            }
        }

        


        walkedCells.Add(labirynthGrid[cursor.x, cursor.y]);

        //int seed = 3493764; //jakies zrodlo seeda
        System.Random rnd = new System.Random(seed);


        int c = 1000;
        while(/*koniec generowania labiryntu*//*labirynthGrid[size.x - 1, size.y - 1].cellType != LabirynthCell.CELL_TYPE.WALKED*/ walkedCells.Count > 0 && c > 0)
        {
            int repeat = rnd.Next(0, 101);
            int randomWalked;
            if(repeat < 5)
            {
                randomWalked = walkedCells.Count - 1;
                cursor = walkedCells[randomWalked].position;
            }
            else
            {
                randomWalked = rnd.Next(0, walkedCells.Count);
                cursor = walkedCells[randomWalked].position;
            }
            

            List<MasterLabirynthCell> neighbours = GetNeighbours(cursor);

            if(neighbours.Count <= 0)
            {
                walkedCells.RemoveAt(randomWalked);
                continue;
            }

            int randomNeighbour = rnd.Next(0, neighbours.Count);

            Debug.LogWarning("selected neighbour: " + randomNeighbour);

            walkedCells.Add(neighbours[randomNeighbour]);

            labirynthGrid[neighbours[randomNeighbour].position.x, neighbours[randomNeighbour].position.y].cellType = LabirynthCell.CELL_TYPE.WALKED;
            

            IntVector2 w = new IntVector2((cursor.x + neighbours[randomNeighbour].position.x) / 2, (cursor.y + neighbours[randomNeighbour].position.y) / 2);

            

            labirynthGrid[w.x, w.y].cellType = LabirynthCell.CELL_TYPE.EMPTY;
            UpdateCellObject(labirynthGrid[neighbours[randomNeighbour].position.x, neighbours[randomNeighbour].position.y], LabirynthCell.CELL_TYPE.WALKED);
            UpdateCellObject(labirynthGrid[w.x, w.y], LabirynthCell.CELL_TYPE.WALKED);
            cursor = walkedCells[walkedCells.Count - 1].position;

           


            c--;

            
        }

        


        Debug.Log("generating thread done: " + c + " " + ((size.x - 1) / 2) * ((size.y - 1) / 2) + " " + walkedCells.Count);


    }

    private List<MasterLabirynthCell> GetNeighbours(IntVector2 location)
    {
        List<MasterLabirynthCell> neighbours = new List<MasterLabirynthCell>();

        IntVector2 dir = new IntVector2();

        for(int i = 0; i < 4; i++)
        {
            switch(i)
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



            if (dir.x < 0 || dir.x >= size.x || dir.y < 0 || dir.y >= size.y)
            {
                continue;

                neighbours = new List<MasterLabirynthCell>();

                switch (i)
                {
                    case 0:
                        if(labirynthGrid[dir.x - 4, dir.y].cellType == LabirynthCell.CELL_TYPE.EMPTY)
                            neighbours.Add(labirynthGrid[dir.x - 4, dir.y]);
                        break;
                    case 1:
                        if (labirynthGrid[dir.x + 4, dir.y].cellType == LabirynthCell.CELL_TYPE.EMPTY)
                            neighbours.Add(labirynthGrid[dir.x + 4, dir.y]);
                        break;
                    case 2:
                        if (labirynthGrid[dir.x, dir.y - 4].cellType == LabirynthCell.CELL_TYPE.EMPTY)
                            neighbours.Add(labirynthGrid[dir.x, dir.y - 4]);
                        break;
                    case 3:
                        if (labirynthGrid[dir.x, dir.y + 4].cellType == LabirynthCell.CELL_TYPE.EMPTY)
                            neighbours.Add(labirynthGrid[dir.x, dir.y + 4]);
                        break;
                }
                    

                break;
            }
            
            
            if (labirynthGrid[dir.x, dir.y].cellType == LabirynthCell.CELL_TYPE.WALL || labirynthGrid[dir.x, dir.y].cellType == LabirynthCell.CELL_TYPE.OBSTICLE || labirynthGrid[dir.x, dir.y].cellType == LabirynthCell.CELL_TYPE.WALKED) continue;

            neighbours.Add(labirynthGrid[dir.x, dir.y]);
        }

        return neighbours;

    }

    public void UpdateCellObject(MasterLabirynthCell cellToSpawn, LabirynthCell.CELL_TYPE changeTo)
    {
        ToSpawnerEvent updateEvent = new ToSpawnerEvent();
        updateEvent.AddListener(parentSpawner.UpdateLabirynthObject);


        cellToSpawn.cellType = changeTo;
        updateEvent.Invoke(cellToSpawn);



        
    }

    public void ExecuteUpdate()
    {
        startUpdate = true;
    }
}
