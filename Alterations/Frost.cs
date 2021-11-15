using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frost : Alteration
{
    int lastAmount = 0;

    private void Start()
    {
        Character character = owner as Character;
        if (character && character.CurrentFight &&
            character.CurrentFight.fighters[character.CurrentFight.Current].character == character)
        {
            character.CurrentFight.EndTurn();
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
        base.OnNewTurn();
        if (ownerChar && ownerChar.CurrentFight != null)
        {
            ownerChar.CurrentFight.EndTurn();
        }
    }

    override protected void Update()
    {
        Character ownerChar = owner as Character;
        if (ownerChar)
        {
            ownerChar.MobilityBonus += lastAmount;
            lastAmount = ownerChar.GetMobility;
            ownerChar.MobilityBonus -= lastAmount;
        }
        base.Update();
    }

    public override void OnTakeDamage(Entity _entity, ref DamageStruct dmg)
    {

    }

    public override void OnWillTakeDamage(Entity _entity, ref DamageStruct dmg)
    {

    }

    override public void Kill()
    {
        base.Kill();
        Character ownerChar = owner as Character;
        if (ownerChar)
        {
            ownerChar.MobilityBonus += lastAmount;
        }
    }
}
