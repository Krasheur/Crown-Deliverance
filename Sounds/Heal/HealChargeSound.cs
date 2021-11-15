using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealChargeSound : MonoBehaviour
{
    void Start()
    {
        AkSoundEngine.PostEvent("HealCharge_Play", gameObject);
    }

    private void OnDestroy()
    {
        AkSoundEngine.PostEvent("HealCharge_Stop", gameObject);
        AkSoundEngine.PostEvent("Heal_Stop", gameObject);
    }
}
