using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector2 targerLocation;

    [SerializeField]
    [Range(0.1f, 0.5f)]
    float movementSmooth = 0.1f;

    CircleCollider2D col;
    Camera cam;

    [SerializeField]
    [Range(-10, 10)]
    float colMargin = 0;

    // Start is called before the first frame update
    void Start()
    {
        targerLocation = transform.position;
        gameObject.AddComponent(typeof(CircleCollider2D));
        col = GetComponent<CircleCollider2D>();

        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        float posX = transform.position.x;
        float posY = transform.position.y;
        targerLocation = new Vector3(Mathf.Lerp(posX, targerLocation.x, movementSmooth), Mathf.Lerp(posY, targerLocation.y, movementSmooth));

        col.radius = GetComponent<CameraGizmos>().size + colMargin;
    }

    public void SetNewTargetLocation(Vector2 newLocation)
    {
        targerLocation = newLocation;
    }


}
