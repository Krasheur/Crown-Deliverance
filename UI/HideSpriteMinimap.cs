using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSpriteMinimap : MonoBehaviour
{
    void Update()
    {
        if (!GetComponentInParent<Character>().isActiveAndEnabled)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
