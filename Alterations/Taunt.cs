using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taunt : Alteration
{
    private void Start()
    {
        if (owner as Character)
        {
            (owner as Character).Target = Emitter;
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

    public override void Kill()
    {
        if (owner as Character && (owner as Character).Target == Emitter)
        {
            (owner as Character).Target = null;
        }
        base.Kill();
    }
}
