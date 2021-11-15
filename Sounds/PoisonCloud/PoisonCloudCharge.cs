using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCloudCharge : MonoBehaviour
{
    void Start()
    {
        AkSoundEngine.PostEvent("PoisonCloudCharge_Play", gameObject);
    }

    private void OnDestroy()
    {
        AkSoundEngine.PostEvent("PoisonCloudCharge_Stop", gameObject);
    }
}
