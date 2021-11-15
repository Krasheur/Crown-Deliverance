using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceChargeSound : MonoBehaviour
{
    void Start()
    {
        AkSoundEngine.PostEvent("IceCharge_Play", gameObject);
    }

    private void OnDestroy()
    {
        AkSoundEngine.PostEvent("IceCharge_Stop", gameObject);
        AkSoundEngine.PostEvent("Ice_Stop", gameObject);
    }
}
