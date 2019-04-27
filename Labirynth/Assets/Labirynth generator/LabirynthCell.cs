using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabirynthCell
{
    public enum CELL_TYPE { OBSTICLE, WALL, EMPTY, WALKED };
    public IntVector2 position { get; protected set; }

    public CELL_TYPE cellType { get;  set; }

    public LabirynthCell()
    {
        
    }

    
   
}
