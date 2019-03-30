using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public LabirynthCell.STATUS status;

    [SerializeField]
    LayerMask camLayerMask;     //mask to cutout detecting smfg different than camera

    [SerializeField]
    bool gizmosAllTheTime = false;      //variable to switch if gizmos should draw all the time or only when object is selected

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("born!");        
    }

    // Update is called once per frame
    void Update()
    {
        //checking if cell collides with camera collider
        Collider2D cam = Physics2D.OverlapBox(transform.position, transform.localScale - new Vector3(0.3f, 0.3f, 0), 0, camLayerMask);

        //switch visibility of cell depends on collides
        if(cam)
        {
            GetComponentInChildren<Renderer>().enabled = true;
        }
        else
        {
            GetComponentInChildren<Renderer>().enabled = false;
        }
    }



    private void OnDrawGizmosSelected()
    {
        
        switch(status)
        {
            case LabirynthCell.STATUS.EMPTY:
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(transform.position, new Vector3(transform.localScale.x, transform.localScale.y, 0));
                break;
            case LabirynthCell.STATUS.USED:
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(transform.position, new Vector3(transform.localScale.x, transform.localScale.y, 0));
                Gizmos.DrawCube(transform.position, new Vector3(transform.localScale.x / 2, transform.localScale.y / 2, 0));
                break;
            case LabirynthCell.STATUS.WALL:
                Gizmos.color = Color.red;
                Gizmos.DrawCube(transform.position, new Vector3(transform.localScale.x, transform.localScale.y, 0));
                Gizmos.color = Color.black;
                Gizmos.DrawWireCube(transform.position, new Vector3(transform.localScale.x, transform.localScale.y, 0));
                break;
        }
        
    }

    private void OnDrawGizmos()
    {

        if (!gizmosAllTheTime) return;

            switch (status)
        {
            case LabirynthCell.STATUS.EMPTY:
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(transform.position, new Vector3(transform.localScale.x, transform.localScale.y, 0));
                break;
            case LabirynthCell.STATUS.USED:
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(transform.position, new Vector3(transform.localScale.x, transform.localScale.y, 0));
                Gizmos.DrawCube(transform.position, new Vector3(transform.localScale.x / 2, transform.localScale.y / 2, 0));
                break;
            case LabirynthCell.STATUS.WALL:
                Gizmos.color = Color.red;
                Gizmos.DrawCube(transform.position, new Vector3(transform.localScale.x, transform.localScale.y, 0));
                Gizmos.color = Color.black;
                Gizmos.DrawWireCube(transform.position, new Vector3(transform.localScale.x, transform.localScale.y, 0));
                break;
        }

    }


}
