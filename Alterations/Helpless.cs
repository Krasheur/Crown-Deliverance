using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helpless : Alteration
{
    Alteration[] alterations;

    private void Start()
    {
        alterations = owner.GetComponentsInChildren<Alteration>();
    }

    protected override void Update()
    {
        Alteration[] tmpAlt = owner.GetComponentsInChildren<Alteration>();

        if (tmpAlt.Length != alterations.Length)
        {
            if (tmpAlt.Length < alterations.Length)
            {
                alterations = tmpAlt;
            }
            else
            {
                Alteration altToRemove = null;
                for (int i = 0; i < tmpAlt.Length; i++)
                {
                    bool found = false;

                    for (int j = 0; j < alterations.Length; j++)
                    {
                        if (tmpAlt[i] == alterations[j])
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        altToRemove = tmpAlt[i];
                        break;
                    }
                }

                if (altToRemove && altToRemove.IsBonus == AlterationEffect.BONUS)
                {
                    altToRemove.Kill();
                }
            }
        }

        base.Update();
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
