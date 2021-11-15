using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowShield : MonoBehaviour
{
    void Start()
    {
        AkSoundEngine.PostEvent("ThrowShield_Play", gameObject);
    }
}
