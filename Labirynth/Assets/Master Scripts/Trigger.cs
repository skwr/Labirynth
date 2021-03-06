﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField]
    List<GameObject> generators = null;

    bool pressed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !pressed)
        {
            pressed = true;
            for (int i = 0; i < generators.Count; i++)
            if(generators[i])
            {
                    //place to start listed generators
                    generators[i].GetComponent<ITrigger>().Execute();
            }
        }
    }
}
