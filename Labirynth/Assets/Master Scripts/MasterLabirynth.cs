using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterLabirynth : LabirynthSpawner
{
    MasterLabirynthGenerator generator;

    

    

    // Start is called before the first frame update
    void Start()
    {
        //get size from somewhere
        
        

        
    }

    public override bool Begin(int seed, IntVector2 size, float sizeMultiplier)
    {

        IntVector2 s = size;

        if (s.x % 2 == 0) s.x++;
        if (s.y % 2 == 0) s.y++;

        gridSize = s;


        if (base.Begin(seed, gridSize, sizeMultiplier))
        {
            generator = new MasterLabirynthGenerator(gridSize, this);
            generator.Generate(seed, MasterLabirynthGenerator.GENERATOR_TYPE.MINOR);
            return true;
        }
        else
        {
            return false;
        }
        
    }

    bool locker = false;

    // Update is called once per frame
    void Update()
    {
        
        base.Update();
    }
}
