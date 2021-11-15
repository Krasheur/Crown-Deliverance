using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowDaggerCharge : MonoBehaviour
{
    void Start()
    {
        AkSoundEngine.PostEvent("ThrowDaggerCharge_Play", gameObject);
    }

    private void OnDestroy()
    {
        AkSoundEngine.PostEvent("ThrowDaggerCharge_Stop", gameObject);

    }
}
