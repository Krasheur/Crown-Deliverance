using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobilityPointLoss : Alteration
{

    private void Start()
    {
        Character ownerChar = owner as Character;
        Character emitterChar = emitter as Character;
        if (ownerChar && emitterChar &&
            (ownerChar.Hostility == emitterChar.Hostility ||
            ownerChar.Hostility == CHARACTER_HOSTILITY.NEUTRAL))
        {
            Destroy(gameObject);
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
        Character ownerChar = owner as Character;
        if (ownerChar)
        {
            ownerChar.MobilityMov = false;
        }

        base.OnNewTurn();
    }

    public override void OnTakeDamage(Entity _entity, ref DamageStruct dmg)
    {

    }

    public override void OnWillTakeDamage(Entity _entity, ref DamageStruct dmg)
    {

    }
}
