using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinorCellObject : RebuildCellObject
{
    [SerializeField]
    SpriteRenderer thisRenderer;

    [SerializeField]
    List<Sprite> sprites = null;
    
    float minorSize;
    int minorDimension;

    float majorSize;
    int majorDimension;

    [SerializeField]
    bool walked = false;


    [SerializeField]
    Material m1, m2;


    private void Start()
    {
        
    }


    MajorCell.CELL_TYPE type;
    public void Initialize(float _minorSize, MajorCell.CELL_TYPE _type)
    {
        //initializing minor object
        //

        thisRenderer = GetComponent<SpriteRenderer>();

        minorSize = _minorSize;



        size = new Vector3(minorSize, minorSize);
        type = _type;

        switch (type)
        {
            case MajorCell.CELL_TYPE.EMPTY:
                //thisRenderer.sprite = sprites[0];
                GetComponent<BoxCollider2D>().enabled = true;
                break;
            case MajorCell.CELL_TYPE.OBSTICLE:
                //thisRenderer.sprite = sprites[0];
                GetComponent<BoxCollider2D>().enabled = true;
                break;
            case MajorCell.CELL_TYPE.PATH:
                thisRenderer.enabled = false;
                break;
            case MajorCell.CELL_TYPE.WALL:
                //thisRenderer.sprite = sprites[0];
                GetComponent<BoxCollider2D>().enabled = true;
                break;
        }

        
        
    }

    private void Update()
    {
        
        size = new Vector3(minorSize * transform.parent.transform.parent.transform.localScale.x, minorSize * transform.parent.transform.parent.transform.localScale.y);

        

        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        thisRenderer.GetPropertyBlock(mpb);

        
        mpb.SetFloat("Vector1_68287AFD", Camera.main.transform.localScale.x);

        thisRenderer.SetPropertyBlock(mpb);

    }

    
}
