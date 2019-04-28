using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterLabirynthObject : LabirynthSpawner
{
    OuterLabirynthGenerator generator;

    // Start is called before the first frame update
    void Start()
    {
        Begin(123, new IntVector2(10, 10), 1);
    }

    // Update is called once per frame
    public override bool Begin(int seed, IntVector2 size, float sizeMultiplier)
    {
        Debug.Log("halo");

        IntVector2 s = size;

        if (s.x % 2 == 0) s.x++;
        if (s.y % 2 == 0) s.y++;

        gridSize = s;


        if (base.Begin(seed, gridSize, sizeMultiplier))
        {
            //base.transform.localScale = new Vector3(base.transform.localScale.x * 10, base.transform.localScale.y * 10, base.transform.localScale.z * 10);
            generator = new OuterLabirynthGenerator(gridSize, this);
            generator.Generate(seed, MasterLabirynthGenerator.GENERATOR_TYPE.MAJOR);
            return true;
        }
        else
        {
            return false;
        }
    }
}
