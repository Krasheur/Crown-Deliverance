using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixRotationMinimapIcon : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(90.0f, 180.0f - transform.parent.transform.rotation.y, 0.0f);
    }
}
