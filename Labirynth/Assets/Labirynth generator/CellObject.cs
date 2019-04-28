using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellObject : MonoBehaviour
{
    public enum CELL_TYPE { WALL, BREAKABLEWALL, PATH, EMPTY, LABIRYNTH };

    [SerializeField]
    CELL_TYPE cellType;

    [SerializeField]
    List<Sprite> visibleSprites;
    [SerializeField]
    List<Sprite> unvisibleSprites;

    [SerializeField]
    GameObject generatorPrefab;

    


    private void Update()
    {
        
    }


    public void SetCellType(CELL_TYPE type)
    {
        cellType = type;

        SpriteRenderer rendererVisible = transform.GetChild(0).GetComponent<SpriteRenderer>();
        SpriteRenderer rendererUnvisible = transform.GetChild(1).GetComponent<SpriteRenderer>();

        switch (cellType)
        {
            case CELL_TYPE.WALL:
                rendererVisible.sprite = visibleSprites[0];
                rendererUnvisible.sprite = unvisibleSprites[0];
                break;
            case CELL_TYPE.BREAKABLEWALL:
                rendererVisible.sprite = visibleSprites[0];
                rendererUnvisible.sprite = unvisibleSprites[0];
                break;
            case CELL_TYPE.PATH:
                rendererVisible.sprite = null;
                rendererUnvisible.sprite = unvisibleSprites[0];
                break;
            case CELL_TYPE.EMPTY:
                rendererVisible.sprite = null;
                rendererUnvisible.sprite = unvisibleSprites[0];
                break;
            case CELL_TYPE.LABIRYNTH:
                rendererVisible.sprite = null;
                rendererUnvisible.sprite = null;

                //spawn generator

                break;
        }
    }


}
