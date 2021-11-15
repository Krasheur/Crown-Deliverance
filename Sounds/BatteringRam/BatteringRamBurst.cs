using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteringRamBurst : MonoBehaviour
{
    void Start()
    {
        AkSoundEngine.PostEvent("BatteringRamBurst_Play", gameObject);
    }
}
