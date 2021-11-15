using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opportunist : Alteration
{
    bool done;
    public override void OnDealDamage(Entity _entity, ref DamageStruct dmgReport)
    {
        done = true;
        if (_entity.GetComponentInChildren<Frost>())
        {
            DamageStruct dmg = new DamageStruct();
            dmg.amountDamag = (int)((dmgReport.amountDamagToArmor + dmgReport.amountDamagToHp) / 2.0f);
            _entity.ChangePv(dmg);
        }
    }

    protected override void Update()
    {
        base.Update();
        if (done)
        {
            Kill();
        }
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
