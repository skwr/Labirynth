using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    LabirynthGenerator labirynth;       //instance of labirynth generator
    

    [SerializeField]
    GameObject cell;        //field for cell prefab used to instantiate

    bool generatingDone = false;        //waiting for generating done flag

    int[,] labirynthGrid;       //basic labirynth shape as int matrix; setting by generator via event

    // Start is called before the first frame update
    void Start()
    {
        labirynth = new LabirynthGenerator(10, 30, new Vector2(0, 0), LabirynthGenerator.GENERATOR.STANDARD, this.gameObject);
        //starting generation process
        labirynth.Generate();
        
        Debug.Log("gamemaster started");
    }

    // Update is called once per frame
    void Update()
    {
        //waiting for flag
        if(generatingDone)
        {
            generatingDone = false; // do it only once; set flag to default state

            //setting up root object for labirynth cells
            GameObject labirynthObject = new GameObject();
            labirynthObject.transform.position = Vector3.zero;

            //instanting cells in labirynth object
            for (int y = 0; y < labirynth.height; y++)
            {
                for (int x = 0; x < labirynth.width; x++)
                {
                    GameObject newCell = (GameObject)Instantiate(cell, new Vector3(x * labirynth.cellSize, y * labirynth.cellSize, 0), Quaternion.Euler(Vector3.zero), labirynthObject.transform);
                    newCell.transform.localScale = new Vector3(labirynth.cellSize, labirynth.cellSize, 1);

                    //setting cell status based on basic labirynth shape matrix
                    newCell.GetComponent<Cell>().status = (LabirynthCell.STATUS)labirynthGrid[x, y];
                }
            }
        }
        
    }

    
    //event handler for generating done event
    public void eventTest(int[,] labirynthGrid)
    {
        //setting instance of basic labitynrh shape matrix
        this.labirynthGrid = labirynthGrid;

        //turning ON flag
        generatingDone = true;
    }
}
