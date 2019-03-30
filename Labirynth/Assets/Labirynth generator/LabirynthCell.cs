using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//basic class to hold template for single labirynth cell
public class LabirynthCell
{
    public enum STATUS {EMPTY, WALL, USED}; //cell status; is it unsigned (EMPTY), set as unuseable (WALL) or set as cell to place smfg

    int x, y;   //cell position in grid
    public STATUS cellStatus;  //variable to hold actual cell status

    

    //constructors
    //////////////////////////////////////////////////////////////////////
    public LabirynthCell()
    {
        x = 0;
        y = 0;
        cellStatus = STATUS.EMPTY;
    }

    public LabirynthCell(int positionX, int positionY)
    {
        x = positionX;
        y = positionY;
        cellStatus = STATUS.EMPTY;
    }

    public LabirynthCell(int positionX, int positionY, STATUS _cellStatus)
    {
        cellStatus = _cellStatus;
    }
    //////////////////////////////////////////////////////////////////////

    public STATUS GetStatus()
    {
        return cellStatus;
    }

    

}
