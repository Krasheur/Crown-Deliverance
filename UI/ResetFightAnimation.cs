using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetFightAnimation : MonoBehaviour
{
    void Update()
    {
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Fight"))
        {
            GetComponent<Animator>().SetBool("FightStarted", false);
        }

    }
}
