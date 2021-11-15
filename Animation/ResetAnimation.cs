using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimation : MonoBehaviour
{
    public void ResetSpellAnimation()
    {        
        GetComponent<Animator>().SetInteger("SpellCasted", -1);
        GetComponent<Animator>().SetInteger("Channeling", -1);
    }

    public void ResetTakeDamage()
    {
        GetComponent<Animator>().SetBool("isTakingDamage", false);
    }
}
