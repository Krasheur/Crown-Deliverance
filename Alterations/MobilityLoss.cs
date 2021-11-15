using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobilityLoss : Alteration
{
    int mobilityLoss = 0;

    protected override string[] format
    {
        get
        {
            return new string[1] { (mobilityLoss).ToString() };
        }
    }

    private void Start()
    {
        Character ownerChar = owner as Character;
        Character emitterChar = emitter as Character;
        if (ownerChar && emitterChar &&
            !(ownerChar.Hostility == emitterChar.Hostility ||
            ownerChar.Hostility == CHARACTER_HOSTILITY.NEUTRAL))
        {
            mobilityLoss = (int)(ownerChar.GetMobility * 0.5f);
            ownerChar.MobilityBonus -= mobilityLoss;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void OnDealDamage(Entity _entity, ref DamageStruct dmgReport)
    {

    }

    public override void OnDeath()
    {

    }

    public override void OnEndTurn()
    {

    }

    public override void OnNewTurn()
    {
        base.OnNewTurn();
    }

    public override void OnTakeDamage(Entity _entity, ref DamageStruct dmg)
    {

    }

    public override void OnWillTakeDamage(Entity _entity, ref DamageStruct dmg)
    {

    }

    public override void Kill()
    {
        base.Kill();
        Character ownerChar = owner as Character;
        if (ownerChar)
        {
            ownerChar.MobilityBonus += mobilityLoss;
        }
    }
}
