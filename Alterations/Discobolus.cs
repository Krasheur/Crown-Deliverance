using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Discobolus : Alteration
{
    Projectile projectile;
    protected bool done;

    public Projectile Projectile { get => projectile; set => projectile = value; }

    protected override string[] format
    {
        get
        {
            return new string[1] { ((int)(BonusFromStat * 0.1f / 4.0f)).ToString() };
        }
    }

    public override void OnDealDamage(Entity _entity, ref DamageStruct dmgReport)
    {
        if (Projectile.HasHit)
        {
            done = true;
            if (dmgReport.dodged) return;
            int debuffNum = (int)(Projectile.Distance / 4.0f);
            DamageStruct dmg = new DamageStruct();
            dmg.amountDamag = (int)(BonusFromStat * 0.1f * debuffNum);
            _entity.ChangePv(dmg);
        }
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
