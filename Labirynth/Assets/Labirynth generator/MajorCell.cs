using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MajorCell : Cell
{
    public enum CELL_TYPE { EMPTY, WALL, OBSTICLE, PATH };

    public CELL_TYPE type;

    public enum DOOR_SITE { LEFT, TOP, RIGHT, BOTTOM};
    public int L, T, R, B;

    

    public MajorCell(IntVector2 _position, CELL_TYPE _type)
    {
        position = _position;
        type = _type;

        L = 0;
        T = 0;
        R = 0;
        B = 0;
    }

    public void SetDoor(DOOR_SITE site, int index)
    {
        

        switch (site)
        {
            case DOOR_SITE.LEFT:
                L = index;
                break;
            case DOOR_SITE.TOP:
                T = index;
                break;
            case DOOR_SITE.RIGHT:
                R = index;
                break;
            case DOOR_SITE.BOTTOM:
                B = index;
                break;
        }
    }
    
}
