using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserk : StatBonus
{

    protected override string[] format
    {
        get
        {
            return new string[2] { base.format[0], percentBonusStat.ToString() };
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
        dmg.amountDamagBonus += (dmg.amountDamagBonus + dmg.amountDamag) * (int)(percentBonusStat / 100.0f);
    }
}
