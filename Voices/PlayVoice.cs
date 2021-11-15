using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayVoice : MonoBehaviour
{
    private enum Kind
    {
        Hymelia,
        Akayel,
        Ryveck,
        Bg
    }

    [SerializeField] string events;
    [SerializeField] Kind who;

    private GameObject sayer;

    private void Awake()
    {
        if(who == Kind.Hymelia) sayer = PlayerManager.main.PlayerFairy.gameObject;
        if(who == Kind.Akayel) sayer = PlayerManager.main.PlayerRogue.gameObject;
        if(who == Kind.Ryveck) sayer = PlayerManager.main.PlayerTank.gameObject;
    }

    void Start()
    {
        if(sayer != null) AkSoundEngine.PostEvent(events, sayer);
    }
}
