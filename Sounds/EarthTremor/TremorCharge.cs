using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TremorCharge : MonoBehaviour
{
    void Start()
    {
        AkSoundEngine.PostEvent("EarthTremorCharge_Play", gameObject);
    }

    private void OnDestroy()
    {
        AkSoundEngine.PostEvent("EarthTremorCharge_Stop", gameObject);
    }
}
