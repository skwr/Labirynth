using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterLabirynthGenerator : MasterLabirynthGenerator
{
    public OuterLabirynthGenerator(IntVector2 _size, LabirynthSpawner _parentSpawner) : base(_size, _parentSpawner)
    {
        Debug.Log("test");
    }
}
