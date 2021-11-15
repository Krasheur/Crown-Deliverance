using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExplosionSound : MonoBehaviour
{
    void Start()
    {
        AkSoundEngine.PostEvent("FireExplosion_Play", gameObject);
    }
}
