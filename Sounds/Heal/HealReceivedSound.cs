using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealReceivedSound : MonoBehaviour
{
    float timer = 0f;

    void Start()
    {
        AkSoundEngine.PostEvent("HealReceived_Play", gameObject);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 2f) Destroy(transform.gameObject);
    }
}
