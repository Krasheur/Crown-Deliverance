using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverHealToArmor : SpellAlteration
{
    [SerializeField] HealDiffToArmor healDiffToArmorPrefab;
    public override void OnFireProjectile(Projectile _projectile)
    {
        if (_projectile.Emitter)
        {
            Instantiate(healDiffToArmorPrefab, _projectile.Emitter.transform);
            HealDiffToArmor alteration = HealDiffToArmor.main;
            Character target = _projectile.Target as Character;
            Character owner = _projectile.Emitter as Character;
            if (target && owner)
            {
                alteration.BaseAmount = _projectile.DamageDescriptor.amountHeal + _projectile.DamageDescriptor.amountHealBonus;
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

    }

    public override void OnSetTarget(Entity _ent, Vector3 _position)
    {

    }
}
