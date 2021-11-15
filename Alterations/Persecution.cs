using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persecution : Alteration
{
    bool hitThisTurn = false;
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
        hitThisTurn = false;
        base.OnNewTurn();
    }

    public override void OnTakeDamage(Entity _entity, ref DamageStruct dmg)
    {
        if (hitThisTurn) return;
        if (emitter && _entity && emitter == _entity)
        {
            Character emitterChar = emitter as Character;
            if (emitterChar)
            {
                hitThisTurn = true;
                emitterChar.CurrentPa += 1;
            }
        }
    }

    public override void OnWillTakeDamage(Entity _entity, ref DamageStruct dmg)
    {

    }
}
