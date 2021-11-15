using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobilityGain : Alteration
{
    int bonus = 0;

    protected override string[] format
    {
        get
        {
            return new string[1] { (bonus).ToString() };
        }
    }

    private void Start()
    {
        Character ownerChar = owner as Character;
        Character emitterChar = emitter as Character;
        if (ownerChar && emitterChar)
        {
            bonus = 1 + (int)(emitterChar.Level * 0.2f);
            ownerChar.MobilityBonus += bonus;
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
            ownerChar.MobilityBonus -= bonus;
        }
    }
}
