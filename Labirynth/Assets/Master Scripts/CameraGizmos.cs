using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class CameraGizmos : MonoBehaviour
{
    [Space]
    [Header("mod by ortographicSize and scaling variable")]
    [Space]
    
    [SerializeField]
    [ReadOnly]
    public float size;

    [SerializeField]
    [Range(100, 0)]
    float scaling = 100;  //

    

    [SerializeField]
    bool gizmosAllTheTime = false;

    [SerializeField]
    GameObject mat;


    private void Update()
    {
        size = GetComponent<Camera>().orthographicSize * (scaling / 100);       //seting size of camera FOV circle

        
    }

    
    private void OnDrawGizmosSelected()
    {
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, 0, -transform.position.z), size);
    }

    private void OnDrawGizmos()
    {
        if(gizmosAllTheTime)
        {
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position + new Vector3(0, 0, -transform.position.z), size);
        }
        
    }
}
