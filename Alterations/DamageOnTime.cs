using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTime : Alteration
{
    [SerializeField] int damage;

    public int Damage { get => damage; set => damage = value; }

    protected override string[] format
    {
        get
        {
            return new string[1] { ((Damage + (int)((Damage * 0.07) * BonusFromStat))).ToString() };
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
        DamageStruct dmg = new DamageStruct();
        dmg.emitter = emitter;
        dmg.amountDamag = (Damage + (int)((Damage * 0.07) * BonusFromStat));
        dmg.touchAutomaticly = true;
        DamageStruct dmgReport = owner.ChangePv(dmg);
        Emitter?.OnDealDamage?.Invoke(owner, ref dmgReport);
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
