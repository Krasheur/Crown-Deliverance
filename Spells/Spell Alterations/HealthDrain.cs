using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrain : SpellAlteration
{
    [SerializeField] StealHealth stealHealthPrefab;
    public override void OnFireProjectile(Projectile _projectile)
    {
        if (_projectile.Emitter)
        {
            Instantiate(stealHealthPrefab, _projectile.Emitter.transform);
            StealHealth alteration = StealHealth.main;
            Character target = _projectile.Target as Character;
            Character owner = _projectile.Emitter as Character;
            if (target && owner)
            {
                int amount = _projectile.DamageDescriptor.amountHeal + _projectile.DamageDescriptor.amountHealBonus;
                DamageStruct dmg = _projectile.DamageDescriptor;
                alteration.TargetNum = owner.SpellCasted.TargetNumberNeeded;
                alteration.DrainAmount = amount;
                dmg.amountHealBonus = dmg.amountHeal = 0;
                dmg.amountDamagBonus = dmg.amountDamag = 0;
                _projectile.DamageDescriptor = dmg;
            }
        }
    }

    public override void OnGetAutomaticHit(ref bool _autHit)
    {

    }

    public override void OnGetDamageAmount(ref int _dmg)
    {

    }

    public override void OnGetHealAmount(ref int _hl)
    {

    }

    public override void OnGetIgnoreArmor(ref bool _ignArm)
    {

    }

    public override void OnGetImpactRadius(ref float _rad)
    {

    }

    public override void OnGetProjectile(ref Projectile _projectile)
    {

    }

    public override void OnGetTargetNeeded(ref int _targNum)
    {
        _targNum++;
    }

    public override void OnSetTarget(Entity _ent, Vector3 _position)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
