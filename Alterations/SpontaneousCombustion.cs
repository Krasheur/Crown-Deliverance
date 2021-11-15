using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpontaneousCombustion : Alteration
{
    bool done;
    public override void OnDealDamage(Entity _entity, ref DamageStruct dmgReport)
    {
        DamageOnTime[] dots = _entity.GetComponentsInChildren<DamageOnTime>(); 
        for (int i = 0; i < dots.Length; i++)
        {
            DamageStruct dmg = new DamageStruct();
            dmg.amountDamag = dots[i].TurnsRemaining * (dots[i].Damage + (int)((dots[i].Damage * 0.07) * dots[i].BonusFromStat));
            dmg.touchAutomaticly = true;
            _entity.ChangePv(dmg);
            dots[i].TurnsRemaining = 0;
            dots[i].Kill();
        }
        done = true;
    }

    protected override void Update()
    {
        base.Update();
        if (done)
        {
            Kill();
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

    }

    public override void OnTakeDamage(Entity _entity, ref DamageStruct dmg)
    {

    }

    public override void OnWillTakeDamage(Entity _entity, ref DamageStruct dmg)
    {

    }
}
