using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrailSound : MonoBehaviour
{
    void Start()
    {
        AkSoundEngine.PostEvent("FireTrail_Play", gameObject);
    }
}
