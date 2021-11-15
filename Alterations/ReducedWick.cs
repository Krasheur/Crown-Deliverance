using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReducedWick : Alteration
{
    bool done;
    public override void OnDealDamage(Entity _entity, ref DamageStruct dmgReport)
    {
        LivingBomb[] livingBombs = _entity.GetComponentsInChildren<LivingBomb>(); 
        for (int i = 0; i < livingBombs.Length; i++)
        {
            livingBombs[i].TurnsRemaining = 0;
            livingBombs[i].Kill();
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
