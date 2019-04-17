using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingCellObjectScript : MonoBehaviour
{
    public void SetEnable(bool enable)
    {
        int c = 1000;
        while (GetComponentInParent<MeshRenderer>() == null && c > 0)
        {
            Debug.Log(c);
            c--;
        }
        GetComponentInParent<MeshRenderer>().enabled = enable;
    }


}
