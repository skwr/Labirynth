﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField]
    MasterLabirynth generator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(generator)
            {
                if(generator.Begin())
                {
                    Debug.Log("generator started");
                }
                else
                {
                    Debug.Log("generator arleady started!");
                }
            }
        }
    }
}
