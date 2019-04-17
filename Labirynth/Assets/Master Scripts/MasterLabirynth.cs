using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterLabirynth : LabirynthSpawner
{
    MasterLabirynthGenerator generator;

    // Start is called before the first frame update
    void Start()
    {
        //get size from somewhere
        IntVector2 s = new IntVector2(10, 10);
        
        if (s.x % 2 == 0) s.x++;
        if (s.y % 2 == 0) s.y++;

        gridSize = s;

        generator = new MasterLabirynthGenerator(gridSize, this);
        generator.Generate();

        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}
