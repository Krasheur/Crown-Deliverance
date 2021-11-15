using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SneakAttackCharge : MonoBehaviour
{
    void Start()
    {
        AkSoundEngine.PostEvent("SneakAttackCharge_Play", gameObject);
    }

    private void OnDestroy()
    {
        AkSoundEngine.PostEvent("SneakAttackCharge_Stop", gameObject);
    }
}
