using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityBurstSound : MonoBehaviour
{
    void Start()
    {
        AkSoundEngine.PostEvent("ElectricityBurst_Play", gameObject);
    }
}
