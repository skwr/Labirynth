using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabirynthCell
{
    public enum TYPE { WALL, WALKABLE, PATH, EMPTY};

    public TYPE type;

    public IntVector2 position;

    public LabirynthCell(IntVector2 _position, TYPE _type)
    {
        type = _type;
        position = _position;
        //Debug.Log("cell created");
    }

    
}
