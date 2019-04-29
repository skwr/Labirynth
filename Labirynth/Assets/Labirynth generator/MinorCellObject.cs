using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinorCellObject : RebuildCellObject
{
    [SerializeField]
    SpriteRenderer thisRenderer = null;

    [SerializeField]
    List<Sprite> sprites = null;
    
    float minorSize;
    int minorDimension;

    float majorSize;
    int majorDimension;

    MajorCell.CELL_TYPE type;
    public void Initialize(float _minorSize, MajorCell.CELL_TYPE _type)
    {
        //initializing minor object
        //

        size = new Vector3(_minorSize, _minorSize);
        type = _type;

        switch (type)
        {
            case MajorCell.CELL_TYPE.EMPTY:

                thisRenderer.sprite = sprites[0];
                break;
            case MajorCell.CELL_TYPE.OBSTICLE:
                thisRenderer.sprite = sprites[0];
                break;
            case MajorCell.CELL_TYPE.PATH:
                thisRenderer.sprite = null;
                break;
            case MajorCell.CELL_TYPE.WALL:
                thisRenderer.sprite = sprites[0];
                break;
        }
    }
}
