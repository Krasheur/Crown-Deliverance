using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricitySound : MonoBehaviour
{
    void Start()
    {
        AkSoundEngine.PostEvent("Electricity_Start", gameObject);
    }

    private void OnDestroy()
    {
        AkSoundEngine.PostEvent("Electricity_Stop", gameObject);
    }
}
