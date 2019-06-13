using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputMenager : MonoBehaviour
{
    [SerializeField]
    [Range(0, 0.5f)]
    float analogDead = 0.1f;

    GameObject gameMenager;
    EventMenager eventMenager;

    private void Awake()
    {
        gameMenager = GameObject.FindGameObjectWithTag("GameMenager");
        if(gameMenager == null)
        {
            enabled = false;
        }
        eventMenager = gameMenager.GetComponent<EventMenager>();
        if(eventMenager == null)
        {
            enabled = false;
        }

        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(Input.GetAxis("LeftAnalogX")) > analogDead || Mathf.Abs(Input.GetAxis("LeftAnalogY")) > analogDead)
        {
            Vector2 leftAnalogDirection = new Vector2(Input.GetAxis("LeftAnalogX"), Input.GetAxis("LeftAnalogY"));
            //Debug.Log("Left analog: " + leftAnalogDirection);
            eventMenager.leftAnalogEvent.Invoke(leftAnalogDirection);
        }

        if (Mathf.Abs(Input.GetAxis("RightAnalogX")) > analogDead || Mathf.Abs(Input.GetAxis("RightAnalogY")) > analogDead)
        {
            Vector2 rightAnalogDirection = new Vector2(Input.GetAxis("RightAnalogX"), Input.GetAxis("RightAnalogY"));
            Debug.Log("Rright analog: " + rightAnalogDirection);
        }

        if(Mathf.Abs(Input.GetAxis("TriggerAxis")) > analogDead)
        {
            Debug.Log("Triggers axis: " + Input.GetAxis("TriggerAxis"));
        }

        if (Input.GetButtonDown("LeftButton"))
        {
            Debug.Log("LeftButton pressed");
            eventMenager.leftButtonPressedEvent.Invoke();
        }

        if (Input.GetButtonDown("RightButton"))
        {
            Debug.Log("RightButton pressed");
        }

        if (Input.GetButtonDown("AButton"))
        {
            Debug.Log("AButton pressed");
        }

        if (Input.GetButtonDown("BButton"))
        {
            Debug.Log("BButton pressed");
        }

        if (Input.GetButtonDown("XButton"))
        {
            Debug.Log("XButton pressed");
        }

        if (Input.GetButtonDown("YButton"))
        {
            Debug.Log("YButton pressed");
        }

    }
}
