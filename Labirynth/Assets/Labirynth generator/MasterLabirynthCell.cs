using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterLabirynthCell : LabirynthCell
{
    public MasterLabirynthCell(IntVector2 _position, CELL_TYPE _type)
    {
        position = _position;
        cellType = _type;
    }

    
}
