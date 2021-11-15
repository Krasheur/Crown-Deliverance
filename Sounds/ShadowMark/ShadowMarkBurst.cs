using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMarkBurst : MonoBehaviour
{
    void Start()
    {
        AkSoundEngine.PostEvent("ShadowMark_Play", gameObject);
    }
}
