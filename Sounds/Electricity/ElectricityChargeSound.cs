using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityChargeSound : MonoBehaviour
{
    void Start()
    {
        AkSoundEngine.PostEvent("ElectricityCharge_Play", gameObject);
    }

    private void OnDestroy()
    {
        AkSoundEngine.PostEvent("ElectricityCharge_Stop", gameObject);
        AkSoundEngine.PostEvent("Electricity_Stop", gameObject);
    }
}
