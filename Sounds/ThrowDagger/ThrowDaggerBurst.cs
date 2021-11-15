using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowDaggerBurst : MonoBehaviour
{
    void Start()
    {
        AkSoundEngine.PostEvent("ThrowDagger_Play", gameObject);
    }
}
