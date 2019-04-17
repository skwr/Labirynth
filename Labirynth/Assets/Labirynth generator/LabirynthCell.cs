using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabirynthCell
{
    public enum CELL_TYPE { OBSTICLE, WALL, EMPTY };
    public IntVector2 position { get; protected set; }

    public CELL_TYPE cellType { get; protected set; }

    public LabirynthCell()
    {
        
    }

    
   
}
