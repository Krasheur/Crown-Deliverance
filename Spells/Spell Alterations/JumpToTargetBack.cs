using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpToTargetBack : SpellAlteration
{
    public override void OnFireProjectile(Projectile _projectile)
    {
        JumpBehindTarget jbt = _projectile.GetComponent<JumpBehindTarget>();
        DamageStruct dmg = _projectile.DamageDescriptor;
        dmg.criticalHit = (spellOwner.Emitter as Character).GetCriticalDamage;
        _projectile.DamageDescriptor = dmg;
        if (jbt)
        {
            ProjectileBounce bounce = _projectile.GetComponent<ProjectileBounce>();
            if (!bounce) bounce = _projectile.gameObject.AddComponent<ProjectileBounce>();

            bounce.BounceNum += 1;
            bounce.BounceRange = 5;
            bounce.BounceTarget = BounceTarget.ENEMY;
        }
        else
        {
            _projectile.gameObject.AddComponent<JumpBehindTarget>();
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
