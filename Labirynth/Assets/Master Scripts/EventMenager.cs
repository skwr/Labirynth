using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventWithVector : UnityEvent<Vector2>
{

}

public class EventMenager : MonoBehaviour
{
    public EventWithVector leftAnalogEvent;
    public EventWithVector rightAnalogEvent;


    // Start is called before the first frame update
    void Awake()
    {
        leftAnalogEvent = new EventWithVector();
        rightAnalogEvent = new EventWithVector();
    }

    
}
