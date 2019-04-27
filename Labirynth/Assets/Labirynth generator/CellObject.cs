using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellObject : MonoBehaviour
{
    public enum CELL_TYPE { WALL, BREAKABLEWALL, PATH};

    CELL_TYPE cellType;

    private bool refreash = false;

    private void Update()
    {
        if(refreash)
        {
            //odpowiednia zmiana rodzaju i wyswietlenie go

            switch (cellType)
            {
                case CELL_TYPE.WALL:
                    GetComponentInParent<MeshRenderer>().enabled = true;
                    break;
                case CELL_TYPE.BREAKABLEWALL:
                    GetComponentInParent<MeshRenderer>().enabled = true;
                    break;
                case CELL_TYPE.PATH:
                    GetComponentInParent<MeshRenderer>().enabled = false;
                    break;
            }

            refreash = false;
        }
    }


    public void SetCellType(CELL_TYPE type)
    {
        cellType = type;
        refreash = true;
    }


}
