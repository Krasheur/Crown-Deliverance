using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEffectVisual : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshFilter>().sharedMesh = transform.parent.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
        transform.localScale = transform.parent.GetChild(0).localScale * 1.01f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
