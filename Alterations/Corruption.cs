﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corruption : Alteration
{
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
        dmg.amountDamagBonus += dmg.amountHeal + dmg.amountHealBonus;
        dmg.amountHealBonus = 0;
        dmg.amountHeal = 0;
    }
}
