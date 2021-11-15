using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortalBlow : Alteration
{
    bool done;
    public override void OnDealDamage(Entity _entity, ref DamageStruct dmgReport)
    {
        done = true;
        if (dmgReport.dodged) return;
        Alteration[] alts = _entity.GetComponentsInChildren<Alteration>();
        int debuffNum = 0;
        for (int i = 0; i < alts.Length; i++)
        {
            debuffNum += (alts[i].IsBonus == AlterationEffect.MALUS) ? 1 : 0;
        }

        DamageStruct dmg = new DamageStruct();
        dmg.amountDamag = (int)(BonusFromStat * 0.1f * debuffNum);
        _entity.ChangePv(dmg);
    }

    protected override void Update()
    {
        Character character = owner as Character;
        base.Update();
        if (done && (!character || character.SpellCasted))
        {
            Kill();
        }
    }

    public override void OnDeath()
    {

    }

    public override void OnEndTurn()
    {
        //Kill();
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
