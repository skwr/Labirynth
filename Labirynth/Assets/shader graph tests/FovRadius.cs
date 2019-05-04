using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FovRadius : MonoBehaviour
{
    SpriteRenderer sr;




    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        sr.GetPropertyBlock(mpb);


        mpb.SetFloat("Vector1_68287AFD", Camera.main.transform.localScale.x);

        sr.SetPropertyBlock(mpb);




    }
}
