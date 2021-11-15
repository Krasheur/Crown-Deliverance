using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMarkCharge : MonoBehaviour
{
    void Start()
    {
        AkSoundEngine.PostEvent("ShadowMarkCharge_Play", gameObject);
    }

    private void OnDestroy()
    {
        AkSoundEngine.PostEvent("ShadowMarkCharge_Stop", gameObject);

    }
}
