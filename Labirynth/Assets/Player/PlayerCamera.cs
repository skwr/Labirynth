using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    Camera cam;     //camera instance
    [SerializeField]
    LayerMask cellMask;     //layer mask to cutout detecting smfg different than cell object

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] cells = Physics2D.OverlapCircleAll(transform.position, 1, cellMask);       //detecting all cells in range of 1

        
        if (cells == null || cells.Length <= 0) return;     //if there is no cells in range, just skip
        
        Collider2D closest = cells[0];      //variable to hold cell that is closest to player, default set to first in detected array

        //checking all cells in detected array for being closest to player
        for(int i = 0; i < cells.Length; i++)
        {
            if(Vector2.Distance(transform.position, closest.transform.position) > Vector2.Distance(transform.position, cells[i].transform.position))
            {
                closest = cells[i];
            }
        }

        //set new camera target location as cell that is closest to player
        cam.GetComponent<CameraController>().SetNewTargetLocation(closest.transform.position);
    }

    //drawing point to visualize position of player
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.25f);
    }
}
