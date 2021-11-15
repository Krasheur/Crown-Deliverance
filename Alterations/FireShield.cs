using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShield : Alteration
{
    [SerializeField] int percentDamageReturned;
    [SerializeField] GameObject fireShield;

    GameObject shield;

    protected override string[] format
    {
        get
        {
            return new string[1] { percentDamageReturned.ToString() };
        }
    }

    void Start()
    {
        shield = Instantiate(fireShield, transform);
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
        if (_entity != emitter)
        {
            DamageStruct dmgReturned = new DamageStruct();
            dmgReturned.amountDamag = (int)((dmg.amountDamagToArmor + dmg.amountDamagToHp) * (percentDamageReturned / 100.0f));
            dmgReturned.emitter = emitter;
            _entity.ChangePv(dmgReturned);
        }
    }

    public override void OnWillTakeDamage(Entity _entity, ref DamageStruct dmg)
    {

    }

    public override void Kill()
    {
        DestroyImmediate(shield, true);

        base.Kill();
    }
}
