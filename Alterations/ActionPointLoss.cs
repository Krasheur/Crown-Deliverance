using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPointLoss : Alteration
{
    [SerializeField] int InitialActionPointLoss;

    protected override string[] format
    {
        get
        {
            return new string[1] { (InitialActionPointLoss > 0 ? "loose " : "gain ") + Mathf.Abs(InitialActionPointLoss).ToString() + (InitialActionPointLoss > 0 ? "" : " extra") };
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
            ownerChar.CurrentPa -= InitialActionPointLoss;
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
