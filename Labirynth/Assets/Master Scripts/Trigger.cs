using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField]
    List<MasterLabirynth> generators;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < generators.Count; i++)
            if(generators[i])
            {
                    System.DateTime now = System.DateTime.Now;

                    int seed = now.Year + now.Month + now.Day + now.Hour + now.Minute + now.Second + now.Millisecond + now.DayOfYear;

                if(generators[i].Begin(seed, new IntVector2(10, 10), 1))
                {
                    Debug.Log("generator " + i + " started");
                }
                else
                {
                    Debug.Log("generator arleady started!");
                }
            }
        }
    }
}
