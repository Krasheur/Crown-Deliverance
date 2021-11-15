using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealDiffToArmor : Alteration
{
    public static HealDiffToArmor main;

    int baseAmount;
    bool canDestroy;
    bool initialised;
    public int BaseAmount { get => baseAmount; set => baseAmount = value; }

    override protected void Awake()
    {
        base.Awake();
        if (main)
        {
            Destroy(gameObject);
        }
        else
        {
            main = this;
        }
    }

    IEnumerator WaitForAllTargets()
    {
        while (!canDestroy)
        {
            canDestroy = true;
            yield return null;
        }
        Destroy(gameObject);
    }

    public override void OnDealDamage(Entity _entity, ref DamageStruct dmgReport)
    {
        Character entityChar = _entity as Character;
        if (entityChar)
        {
            entityChar.GiveArmor(baseAmount - dmgReport.amountHeal);
        }
        canDestroy = false;
        if (!initialised)
        {
            initialised = true;
            StartCoroutine(WaitForAllTargets());
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

    public override void Kill()
    {
        base.Kill();
        if (main == this)
        {
            main = null;
        }
    }

    public override void OnWillTakeDamage(Entity _entity, ref DamageStruct dmg)
    {

    }
}
