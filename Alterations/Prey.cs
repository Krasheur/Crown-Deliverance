using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prey : Alteration
{
    protected override string[] format
    {
        get
        {
            return new string[1] { ((int)(BonusFromStat * 0.01f * 100.0f)).ToString() };
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
        if (_entity && emitter && _entity == emitter)
        {
            dmg.amountDamagBonus += (int)Mathf.Ceil((dmg.amountDamag + dmg.amountDamagBonus) * BonusFromStat * 0.01f);
        }
    }
}
