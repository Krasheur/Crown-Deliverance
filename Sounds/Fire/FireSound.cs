using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSound : MonoBehaviour
{
    void Start()
    {
        AkSoundEngine.PostEvent("Fire_Start", gameObject);
    }

    private void OnDestroy()
    {
        AkSoundEngine.PostEvent("Fire_Stop", gameObject);
    }
}
