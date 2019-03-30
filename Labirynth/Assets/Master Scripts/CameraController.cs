using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector2 targerLocation;

    [SerializeField]
    [Range(0.1f, 0.5f)]
    float movementSmooth = 0.1f;

    CircleCollider2D col;       //instance of collider for seting size of collider; collider is use by cells to detect that they are colide
    

    [SerializeField]
    [Range(-100, 100)]
    float colMargin = 0;

    // Start is called before the first frame update
    void Start()
    {
        targerLocation = transform.position;
        gameObject.AddComponent(typeof(CircleCollider2D));
        col = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float posX = transform.position.x;
        float posY = transform.position.y;

        //moveing camera to new position using Linear Interpolation
        transform.position = new Vector3(Mathf.Lerp(posX, targerLocation.x, movementSmooth), Mathf.Lerp(posY, targerLocation.y, movementSmooth), -10);

        //updating size of collider based on margin percentage
        col.radius = GetComponent<CameraGizmos>().size + (colMargin * GetComponent<CameraGizmos>().size)/100;
    }

    //public method to call from outside
    public void SetNewTargetLocation(Vector2 newLocation)
    {
        targerLocation = newLocation;
    }


}
