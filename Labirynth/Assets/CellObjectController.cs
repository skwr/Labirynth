using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellObjectController : MonoBehaviour
{
    [SerializeField]
    bool visible = false;

    bool lastState;

    SpriteRenderer sr;

    float fovRadius;


    // Start is called before the first frame update
    void Start()
    {
        fovRadius = Camera.main.transform.localScale.x;
        lastState = visible;
        sr = GetComponent<SpriteRenderer>();

        
    }

    // Update is called once per frame
    void Update()
    {
        if(lastState != visible)
        {
            lastState = visible;

            

            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            sr.GetPropertyBlock(mpb);

            if (visible)
            {
                mpb.SetFloat("Vector1_68287AFD", 1);
            }
            else
            {
                mpb.SetFloat("Vector1_68287AFD", fovRadius);
            }
            
            

            sr.SetPropertyBlock(mpb);

        }

        if(fovRadius != Camera.main.transform.localScale.x)
        {
            fovRadius = Camera.main.transform.localScale.x;

            if (!visible)
            {
                MaterialPropertyBlock mpb = new MaterialPropertyBlock();
                sr.GetPropertyBlock(mpb);


                mpb.SetFloat("Vector1_68287AFD", fovRadius);


                sr.SetPropertyBlock(mpb);
            }
            
        }
    }

    
}
