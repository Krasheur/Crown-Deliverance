using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCloudBurst : MonoBehaviour
{
    void Start()
    {
        AkSoundEngine.PostEvent("PoisonCloud_Play", gameObject);
    }
}
