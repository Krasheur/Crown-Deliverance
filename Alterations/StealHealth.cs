using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealHealth : Alteration
{
    public static StealHealth main;
    List<Entity> entityToHeal;
    List<Entity> entityToDrain;
    int targetNum = 0;
    int targetHitNum = 0;
    int hpDrained = 0;
    int drainAmount = 0;
    bool canDrain = false;
    bool initialised = false;

    public int TargetNum { get => targetNum; set => targetNum = value; }
    public int DrainAmount { get => drainAmount; set => drainAmount = value; }

    override protected void Awake()
    {
        base.Awake();
        if (main)
        {
            Kill();
        }
        else
        {
            main = this;
            entityToHeal = new List<Entity>();
            entityToDrain = new List<Entity>();
        }
    }

    public override void OnDeath()
    {

    }

    public override void OnEndTurn()
    {
        Kill();
    }

    public override void OnNewTurn()
    {

    }

    IEnumerator WaitForAllTargets()
    {
        while (!canDrain)
        {
            canDrain = true;
            yield return null;
        }

        owner.OnDealDamage -= OnDealDamage;
        for (int i = 0; i < entityToDrain.Count; i++)
        {
            DamageStruct dmg = new DamageStruct();
            dmg.amountHeal = 0;
            dmg.amountHealBonus = 0;
            dmg.amountDamag = drainAmount;
            dmg.amountDamagBonus = 0;
            dmg.criticalHit = -1;
            dmg.emitter = owner;
            dmg.touchAutomaticly = true;
            dmg = (entityToDrain[i] as Character).ChangePv(dmg);
            hpDrained += dmg.amountDamagToArmor + dmg.amountDamagToHp;
            emitter?.OnDealDamage?.Invoke(entityToDrain[i], ref dmg);
        }
        for (int i = 0; i < entityToHeal.Count; i++)
        {
            DamageStruct dmg = new DamageStruct();
            dmg.amountHeal = hpDrained;
            dmg.amountHealBonus = 0;
            dmg.amountDamag = 0;
            dmg.amountDamagBonus = 0;
            dmg.criticalHit = -1;
            dmg.emitter = owner;
            dmg.touchAutomaticly = true;
            dmg = (entityToHeal[i] as Character).ChangePv(dmg);
            emitter?.OnDealDamage?.Invoke(entityToHeal[i], ref dmg);
        }
        Kill();
    }

    public override void OnDealDamage(Entity _entity, ref DamageStruct dmgReport)
    {
        Character ownerChar = owner as Character;
        Character entityChar = _entity as Character;

        if (ownerChar && entityChar)
        {
            if (entityChar.Hostility == ownerChar.Hostility || entityChar.Hostility == CHARACTER_HOSTILITY.NEUTRAL)
            {
                if (!entityToHeal.Contains(entityChar)) entityToHeal.Add(entityChar);
            }
            else if (!dmgReport.dodged)
            {
                if (!entityToDrain.Contains(entityChar)) entityToDrain.Add(entityChar);
            }
        }
        canDrain = false;
        if (!initialised)
        {
            initialised = true;
            StartCoroutine(WaitForAllTargets());
        }
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
