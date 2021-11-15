using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TremorBurst : MonoBehaviour
{
    void Start()
    {
        AkSoundEngine.PostEvent("EarthTremor_Play", gameObject);
    }
}
