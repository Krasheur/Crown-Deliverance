using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveDagger : MonoBehaviour
{
    void Start()
    {
        AkSoundEngine.PostEvent("ReceiveDagger_Play", gameObject);
    }
}
