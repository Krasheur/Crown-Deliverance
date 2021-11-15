using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScapeGoat : Alteration
{
    Entity target;

    public override void OnDealDamage(Entity _entity, ref DamageStruct dmgReport)
    {
        if (!target)
        {
            target = _entity;
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
        base.OnNewTurn();
    }

    public override void OnTakeDamage(Entity _entity, ref DamageStruct dmg)
    {
        if (target)
        {
            DamageStruct newDmg = dmg;
            target.ChangePv(newDmg);
        }
    }

    public override void OnWillTakeDamage(Entity _entity, ref DamageStruct dmg)
    {

    }
}
