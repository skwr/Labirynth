using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNumbersGenerator : MonoBehaviour
{
    [SerializeField]
    int seed;

    System.Random rnd;

    // Start is called before the first frame update
    void Start()
    {
        if(seed == 0)
        {
            //TO DO:
            //prepare seed from year, month, day, hour, min, sec, millis
            seed = (DateTime.Now.Year * DateTime.Now.Month) + (DateTime.Now.Day * DateTime.Now.Hour) * DateTime.Now.Minute * DateTime.Now.Second * DateTime.Now.Millisecond;
            //seed = 123; //temporary, change to ^
        }

        rnd = new System.Random(seed);
    }

    

    public int GetRandomNumber(int min, int max)
    {
        //returning requested random number
        return rnd.Next(min, max);
    }

    
}
