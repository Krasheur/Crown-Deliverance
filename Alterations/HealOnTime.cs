using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOnTime : Alteration
{
    [SerializeField] int heal;
    [SerializeField] GameObject healFX;

    protected override string[] format
    {
        get
        {
            return new string[1] { ((heal + (int)((heal * 0.07) * BonusFromStat))).ToString() };
        }
    }

    public override void OnDeath()
    {

    }

    public override void OnEndTurn()
    {

    }

    public override void OnNewTurn()
    {
        DamageStruct dmg = new DamageStruct();
        dmg.amountHeal = (heal + (int)((heal * 0.07) * BonusFromStat));
        dmg.touchAutomaticly = true;
        DamageStruct dmgReport = owner.ChangePv(dmg);
        Emitter?.OnDealDamage?.Invoke(owner, ref dmgReport);

        Instantiate(healFX, transform);

        base.OnNewTurn();
    }

    public override void OnDealDamage(Entity _entity, ref DamageStruct dmgReport)
    {

    }

    public override void OnTakeDamage(Entity _entity, ref DamageStruct dmg)
    {

    }

    public override void OnWillTakeDamage(Entity _entity, ref DamageStruct dmg)
    {

    }
}
