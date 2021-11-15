using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostRepairArmor : Frost
{
    public override void OnWillTakeDamage(Entity _entity, ref DamageStruct dmg)
    {
        DamageStruct newDmg = dmg;
        newDmg.amountDamag = 0;
        newDmg.amountDamagBonus = 0;
        dmg = newDmg;
    }

    override public void Kill()
    {
        base.Kill();
        Character ownerChar = owner as Character;
        if (TurnsRemaining <= 0 && ownerChar)
        {
            DamageStruct dmg = new DamageStruct();
            dmg.amountArmor = ownerChar.ArmorMax;
            ownerChar.ChangePv(dmg);
        }
    }
}
