using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendableContract : Alteration
{
    SpellHolder spellHolder;

    private void Start()
    {
        Character emitterChar = emitter as Character;
        spellHolder = emitterChar.SpellList[2];
    }

    public override void OnDealDamage(Entity _entity, ref DamageStruct dmgReport)
    {

    }

    public override void OnDeath()
    {
        spellHolder.TurnsRemaining = 0;
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
