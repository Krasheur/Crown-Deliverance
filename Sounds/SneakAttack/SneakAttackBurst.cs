using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SneakAttackBurst : MonoBehaviour
{
    void Start()
    {
        AkSoundEngine.PostEvent("SneakAttack_Play", gameObject);
    }
}
