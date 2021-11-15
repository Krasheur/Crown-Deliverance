using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protected : Alteration
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
        if (emitter && !emitter.IsDead)
        {
            DamageStruct newDmg = new DamageStruct();
            int amount = (dmg.amountDamag + dmg.amountDamagBonus) / 2;
            dmg.amountDamag /= 2;
            dmg.amountDamagBonus /= 2;
            newDmg.amountDamag = amount;
            emitter.ChangePv(newDmg);
        }
    }


}
