using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealWeakPoints : Alteration
{
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
        if (emitter && _entity && emitter == _entity)
        {
            Character emitterChar = (emitter as Character);
            if (emitterChar && dmg.criticalHit <= 0)
            {
                DamageStruct newDmg = dmg;
                newDmg.criticalHit = (Random.Range(0, 100) < emitterChar.GetCriticalChance * 1.2f) ? emitterChar.GetCriticalDamage : newDmg.criticalHit;
                dmg = newDmg;
            }
        }
    }
}
