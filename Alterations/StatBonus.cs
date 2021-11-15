using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBonus : Alteration
{
    [SerializeField] CharacterStat stat;
    [SerializeField] protected int percentBonusStat;
    int bonus;

    protected override string[] format
    {
        get
        {
            return new string[1] { bonus.ToString() };
        }
    }

    private void Start()
    {
        bonus = (percentBonusStat > 0 ? 1 : -1) + (int)(BonusFromStat * (percentBonusStat / 100.0f));
        if (owner as Character)
        {
            switch (stat)
            {
                case CharacterStat.DEXTERITY:
                    (owner as Character).DexterityBonus += bonus;
                    break;
                case CharacterStat.STRENGTH:
                    (owner as Character).StrengthBonus += bonus;
                    break;
                case CharacterStat.INTELLIGENCE:
                    (owner as Character).IntelligenceBonus += bonus;
                    break;
                case CharacterStat.CONSTITUTION:
                    (owner as Character).ConstitutionBonus += bonus;
                    break;
                default:
                    break;
            }
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
            switch (stat)
            {
                case CharacterStat.DEXTERITY:
                    (owner as Character).DexterityBonus -= bonus;
                    break;
                case CharacterStat.STRENGTH:
                    (owner as Character).StrengthBonus -= bonus;
                    break;
                case CharacterStat.INTELLIGENCE:
                    (owner as Character).IntelligenceBonus -= bonus;
                    break;
                case CharacterStat.CONSTITUTION:
                    (owner as Character).ConstitutionBonus -= bonus;
                    break;
                default:
                    break;
            }
        }
        base.Kill();
    }
}
