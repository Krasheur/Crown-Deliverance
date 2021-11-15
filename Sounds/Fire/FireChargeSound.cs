using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireChargeSound : MonoBehaviour
{
    void Start()
    {
        AkSoundEngine.PostEvent("FireCharge_Play", gameObject);
    }

    private void OnDestroy()
    {
        AkSoundEngine.PostEvent("FireCharge_Stop", gameObject);
        AkSoundEngine.PostEvent("Fire_Stop", gameObject);
    }
}
