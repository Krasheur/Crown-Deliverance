using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearAlteration : Alteration
{


    private void Start()
    {
        Character character = owner as Character;
        Character emitterChar = emitter as Character;
        if (character != null && emitterChar != null)
        {
            for (int i = 0; i < owner.transform.childCount; i++)
            {
                Alteration alt = owner.transform.GetChild(i).GetComponent<Alteration>();
                bool isAlly = (character.Hostility == emitterChar.Hostility || character.Hostility == CHARACTER_HOSTILITY.NEUTRAL);
                if (alt && alt != this &&
                    ((alt.IsBonus == AlterationEffect.MALUS && isAlly) ||
                    (alt.IsBonus == AlterationEffect.BONUS && !isAlly)
                    ))
                {
                    alt.Kill();
                }
            }
        }
        Kill();
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

    public override void OnDealDamage(Entity _entity, ref DamageStruct dmgReport)
    {

    }

    public override void OnTakeDamage(Entity _entity, ref DamageStruct dmg)
    {

    }

    public override void OnWillTakeDamage(Entity _entity, ref DamageStruct dmg)
    {

    }
}
