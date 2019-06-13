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

    private bool rolling = false;
    private float rollingCounter = 0;
    [SerializeField]
    private float rollingTime = 0;

    private float rollVel = 8;

    private int playerDir = 0;

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

        eventMenager.leftButtonPressedEvent.AddListener(Roll);
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("vel", Mathf.Abs(rigid.velocity.x));

        if (rolling)
        {
            if(Mathf.Abs(rigid.velocity.x) < 5)
            {
                rigid.velocity = new Vector3(rigid.velocity.x / 2, 0, 0);
                rolling = false;
                GetComponent<Animator>().SetBool("roll", false);
                rollingCounter = 0;
                eventMenager.leftAnalogEvent.AddListener(Move);
            }
        }
    }

    void Move(Vector2 dir)
    {
        rigid.velocity = new Vector2(maxVel * dir.x, 0);
        if(dir.x > 0)
        {
            playerDir = 1;
        }else if(dir.x < 0)
        {
            playerDir = -1;
        }

        if(dir.x > 0)
        {
            transform.rotation = Quaternion.Euler(Vector2.zero);
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector2(0, 180));
        }
    }

    void Roll()
    {
        rolling = true;
        GetComponent<Animator>().SetBool("roll", true);
        eventMenager.leftAnalogEvent.RemoveListener(Move);
        rigid.velocity = new Vector2(rollVel * playerDir, 0);
    }
}
