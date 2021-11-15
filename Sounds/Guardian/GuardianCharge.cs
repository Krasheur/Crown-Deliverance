using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianCharge : MonoBehaviour
{
    void Start()
    {
        AkSoundEngine.PostEvent("GuardianCharge_Play", gameObject);
    }

    private void OnDestroy()
    {
        AkSoundEngine.PostEvent("GuardianCharge_Stop", gameObject);
    }
}
