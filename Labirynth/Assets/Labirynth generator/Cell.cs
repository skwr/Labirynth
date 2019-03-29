using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public LabirynthCell.STATUS status;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

    
}
