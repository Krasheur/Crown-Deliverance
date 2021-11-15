using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheMoreTheMerrier : Alteration
{
    int nbNearbyEnemy = 0;
    BonusDamage damageBoost;
    Spell spellCasted;

    protected override string[] format
    {
        get
        {
            return new string[1] { (nbNearbyEnemy * 10).ToString() };
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

    override protected void Update()
    {
        Character ownerChar = owner as Character;
        if (ownerChar && spellCasted != ownerChar.SpellCasted)
        {
            spellCasted = ownerChar.SpellCasted;
            if (spellCasted)
            {
                Instantiate(damageBoost, spellCasted.transform).PercentBonusDamage = nbNearbyEnemy * 10;
            }
        }
        base.Update();
    }

    public override void OnTakeDamage(Entity _entity, ref DamageStruct dmg)
    {

    }

    public override void OnWillTakeDamage(Entity _entity, ref DamageStruct dmg)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Character targetChar;
        Character ownerChar = owner as Character;
        if (ownerChar && other.TryGetComponent(out targetChar) && targetChar.Hostility != ownerChar.Hostility)
        {
            nbNearbyEnemy++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Character targetChar;
        Character ownerChar = owner as Character;
        if (ownerChar && other.TryGetComponent(out targetChar) && targetChar.Hostility != ownerChar.Hostility)
        {
            nbNearbyEnemy--;
        }
    }
}
