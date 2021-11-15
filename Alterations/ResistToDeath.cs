using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResistToDeath : Alteration
{
    bool wasKillable;

    protected override void Awake()
    {
        base.Awake();
        wasKillable = owner.IsKillable;
        owner.IsKillable = false;
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

    public override void Kill()
    {
        base.Kill();
        owner.IsKillable = wasKillable;
    }

    public override void OnWillTakeDamage(Entity _entity, ref DamageStruct dmg)
    {

    }
}
