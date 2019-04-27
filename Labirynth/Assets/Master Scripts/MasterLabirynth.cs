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
        
        IntVector2 s = new IntVector2(30, 30);
        
        if (s.x % 2 == 0) s.x++;
        if (s.y % 2 == 0) s.y++;

        gridSize = s;

        
    }

    public override bool Begin()
    {
        
        
        

        if(base.Begin())
        {
            generator = new MasterLabirynthGenerator(gridSize, this);
            generator.Generate(1125);
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
        
       /* bool stoped = base.Begin();

        if(!locker)
        {
            if (!stoped)
            {
                
            }
            else
            {
                Debug.Log("Generator stopped...");
            }
        }
        */
        

        base.Update();
    }
}
