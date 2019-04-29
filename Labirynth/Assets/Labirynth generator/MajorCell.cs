using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MajorCell : Cell
{
    public enum CELL_TYPE { EMPTY, WALL, OBSTICLE, PATH };

    public CELL_TYPE type;

    public MajorCell(IntVector2 _position, CELL_TYPE _type)
    {
        position = _position;
        type = _type;

    }
}
