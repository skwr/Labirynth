using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RebuildCellObject : MonoBehaviour
{
    protected Vector3 size;


    //create here some sort of another spawner, wich will be spawn mobs, pickups etc.



    

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y, transform.position.z), size);
    }
}
