using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodLust : Alteration
{
    protected override string[] format
    {
        get
        {
            return new string[1] { ((int)Mathf.Ceil(BonusFromStat * 0.2f)).ToString() };
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
        if (emitter && owner)
        {
            int amount = (int)Mathf.Ceil(BonusFromStat * 0.2f);
            DamageStruct dmg = new DamageStruct();
            dmg.amountDamag = amount;
            DamageStruct heal = new DamageStruct();
            heal.amountHeal = amount;

            emitter.ChangePv(heal);
            owner.ChangePv(dmg);
        }
        base.OnNewTurn();
    }

    public override void OnTakeDamage(Entity _entity, ref DamageStruct dmg)
    {

    }

    public override void OnWillTakeDamage(Entity _entity, ref DamageStruct dmg)
    {

    }
}
