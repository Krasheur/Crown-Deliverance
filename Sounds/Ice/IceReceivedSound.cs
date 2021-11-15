using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceReceivedSound : MonoBehaviour
{
    float timer = 0f;

    void Start()
    {
        AkSoundEngine.PostEvent("IceReceived_Play", gameObject);
    }

    //void Update()
    //{
    //    timer += Time.deltaTime;

    //    //if (timer > 2f) Destroy(transform.gameObject);
    //}
}
