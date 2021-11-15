using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveShield : MonoBehaviour
{
    void Start()
    {
        AkSoundEngine.PostEvent("ReceiveShield_Play", gameObject);
    }
}
