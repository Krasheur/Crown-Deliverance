using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeBonus : Alteration
{
    [SerializeField] int percentBonus;
    int bonus;

    protected override string[] format
    {
        get
        {
            return new string[1] { (bonus).ToString() };
        }
    }

    private void Start()
    {
        bonus = (percentBonus > 0 ? 1 : -1) + (int)(BonusFromStat * (percentBonus / 100.0f));
        if (owner as Character)
        {
            (owner as Character).DodgeBonus += bonus;
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
        base.OnNewTurn();
    }

    public override void OnTakeDamage(Entity _entity, ref DamageStruct dmg)
    {

    }

    public override void OnWillTakeDamage(Entity _entity, ref DamageStruct dmg)
    {

    }

    public override void Kill()
    {
        if (owner as Character)
        {
            (owner as Character).DodgeBonus -= bonus;
        }
        base.Kill();
    }
}
