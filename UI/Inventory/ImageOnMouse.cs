using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageOnMouse : MonoBehaviour
{
    private Pickable item;

    public Pickable Item { get => item; set => item = value; }

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
    }
}
