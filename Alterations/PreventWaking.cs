using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreventWaking : Alteration
{
    protected override void Update()
    {
        base.Update();

        Sleep sleep = owner.GetComponentInChildren<Sleep>();

        if (sleep)
        {
            sleep.CanWake = false;
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
}
