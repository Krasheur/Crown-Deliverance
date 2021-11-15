using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetDuration : Alteration
{
    private void Start()
    {
        Character emitterChar = emitter as Character;
        Character ownerChar = owner as Character;
        bool isAlly = false;
        if (emitterChar && ownerChar)
        {
            isAlly = (ownerChar.Hostility == emitterChar.Hostility || ownerChar.Hostility == CHARACTER_HOSTILITY.NEUTRAL);
        }

        if (isBonus != AlterationEffect.BOTH && isAlly == (isBonus == AlterationEffect.BONUS))
        {
            if (isAlly)
            {
                for (int i = 0; i < owner.transform.childCount; i++)
                {
                    Alteration alteration = owner.transform.GetChild(i).GetComponent<Alteration>();
                    if (alteration && alteration.IsBonus == AlterationEffect.BONUS)
                    {
                        alteration.TurnsRemaining += alteration.Duration;
                    }
                }
            }
            else
            {
                for (int i = 0; i < owner.transform.childCount; i++)
                {
                    Alteration alteration = owner.transform.GetChild(i).GetComponent<Alteration>();
                    if (alteration && alteration.IsBonus == AlterationEffect.MALUS)
                    {
                        alteration.TurnsRemaining += alteration.Duration;
                    }
                }
            }
        }

        Kill();
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

    }

    public override void OnTakeDamage(Entity _entity, ref DamageStruct dmg)
    {

    }

    public override void OnWillTakeDamage(Entity _entity, ref DamageStruct dmg)
    {

    }
}
