using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    GameObject gameMenager;
    EventMenager eventMenager;

    Rigidbody2D rigid;

    Animator anim;

    float maxVel = 3;

    private void Awake()
    {
        gameMenager = GameObject.FindGameObjectWithTag("GameMenager");
        if (gameMenager == null)
        {
            enabled = false;
        }
        eventMenager = gameMenager.GetComponent<EventMenager>();
        if (eventMenager == null)
        {
            enabled = false;
        }

        
    }

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        eventMenager.leftAnalogEvent.AddListener(Move);
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("vel", Mathf.Abs(rigid.velocity.x));
    }

    void Move(Vector2 dir)
    {
        rigid.velocity = new Vector2(maxVel * dir.x, 0);
        if(dir.x > 0)
        {
            transform.rotation = Quaternion.Euler(Vector2.zero);
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector2(0, 180));
        }
    }
}
