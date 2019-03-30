using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    LabirynthGenerator labirynth;

    bool generatingDone = false;

    [SerializeField]
    GameObject cell;

    [SerializeField]
    CameraController cameraObject;

    // Start is called before the first frame update
    void Start()
    {
        labirynth = new LabirynthGenerator(10, 30, new Vector2(0, 0), LabirynthGenerator.GENERATOR.STANDARD);
        labirynth.Generate();
        Thread waitForGenerateThread = new Thread(new ThreadStart(WaitForGenerateThread));
        waitForGenerateThread.Start();

        Debug.Log("gamemaster started");
    }

    // Update is called once per frame
    void Update()
    {
        if(generatingDone)
        {
            GameObject cellsParent = new GameObject();
            Debug.Log("starting instantiate");

            

            generatingDone = false;
            for(int y = 0; y < labirynth.height; y++)
            {
                for(int x = 0; x < labirynth.width; x++)
                {
                    GameObject newCell = (GameObject)Instantiate(cell, new Vector3(x * labirynth.cellSize, y * labirynth.cellSize, 0), Quaternion.Euler(Vector3.zero), cellsParent.transform);
                    newCell.transform.localScale = new Vector3(labirynth.cellSize, labirynth.cellSize, 1);
                    newCell.GetComponent<Cell>().status = labirynth.labirynthGrid[x, y].cellStatus;
                }
            }

            
        }
    }

    void WaitForGenerateThread()
    {
        Debug.Log("waitin for generating done");
        while(!labirynth.done)
        {

        }

        generatingDone = true;
    }
}
