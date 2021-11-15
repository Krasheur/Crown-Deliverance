using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorOnTime : Alteration
{
    [SerializeField] int armor;
    [SerializeField] GameObject armorFX;

    GameObject fx;

    protected override string[] format
    {
        get
        {
            return new string[1] { ((armor + (int)(armor * 0.07) * BonusFromStat)).ToString() };
        }
    }

    void Start()
    {
        fx = Instantiate(armorFX, transform);
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
        dmg.amountArmor = (armor + (int)(armor * 0.07) * BonusFromStat);
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

    public override void Kill()
    {
        DestroyImmediate(fx, true);

        base.Kill();
    }
}
