using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianBurst : MonoBehaviour
{
    void Start()
    {
        AkSoundEngine.PostEvent("Guardian_Play", gameObject);
    }
}
