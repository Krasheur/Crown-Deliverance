using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingBomb : Alteration
{
    [SerializeField] Projectile bomb;
    [SerializeField] GameObject fireBomb;

    GameObject firebomb;

    protected override string[] format
    {
        get
        {
            return new string[1] { ((bomb.Damage + (int)Mathf.Ceil((bomb.Damage * 0.07f) * BonusFromStat))).ToString() };
        }
    }

    void Start()
    {
        firebomb = Instantiate(fireBomb, transform);
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
        turnsRemaining = 0;
        Kill();
    }

    public override void OnTakeDamage(Entity _entity, ref DamageStruct dmg)
    {

    }

    public override void OnWillTakeDamage(Entity _entity, ref DamageStruct dmg)
    {

    }

    DamageStruct GetDamageStruct()
    {
        DamageStruct damageDescriptor = new DamageStruct();
        damageDescriptor.amountDamag = (bomb.Damage + (int)Mathf.Ceil((bomb.Damage * 0.07f) * BonusFromStat));
        damageDescriptor.emitter = emitter;
        damageDescriptor.percArmor = false;
        damageDescriptor.touchAutomaticly = false;
        return damageDescriptor;
    }

    public override void Kill()
    {
        DestroyImmediate(firebomb, true);

        if (turnsRemaining <= 0)
        {
            Projectile newProj = Instantiate(bomb, transform.position, transform.rotation);
            newProj.Target = null;
            newProj.TargetPosition = transform.position;
            newProj.Emitter = emitter;
            newProj.TargetingMethod = TargetingMethod.POSITION_ENTITIES;
            newProj.DamageDescriptor = GetDamageStruct();
        }
        base.Kill();
    }
}
