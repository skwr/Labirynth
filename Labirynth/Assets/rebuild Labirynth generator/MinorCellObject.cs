using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinorCellObject : RebuildCellObject
{
    [SerializeField]
    SpriteRenderer renderer;

    [SerializeField]
    List<Sprite> sprites;

    bool generatingDone = false;
    float minorSize;
    int minorDimension;

    float majorSize;
    int majorDimension;

    MajorCell.CELL_TYPE type;
    public void Initialize(float _minorSize, MajorCell.CELL_TYPE _type)
    {
        size = new Vector3(_minorSize, _minorSize);
        type = _type;

        switch (type)
        {
            case MajorCell.CELL_TYPE.EMPTY:

                renderer.sprite = sprites[0];
                break;
            case MajorCell.CELL_TYPE.OBSTICLE:
                renderer.sprite = sprites[0];
                break;
            case MajorCell.CELL_TYPE.PATH:
                renderer.sprite = null;
                break;
            case MajorCell.CELL_TYPE.WALL:
                renderer.sprite = sprites[0];
                break;
        }
    }
}
