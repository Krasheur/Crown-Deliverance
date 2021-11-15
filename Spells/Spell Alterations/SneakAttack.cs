using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SneakAttack : SpellAlteration
{
    public override void OnFireProjectile(Projectile _projectile)
    {
        if (_projectile.Target)
        {
            Vector2 direction = new Vector2(_projectile.Target.transform.position.x - transform.position.x, _projectile.Target.transform.position.z - transform.position.z);
            Vector2 targetForward = new Vector2(_projectile.Target.transform.forward.x, _projectile.Target.transform.forward.z);
            if (Vector2.Dot(direction.normalized, targetForward.normalized) > Mathf.Cos(Mathf.PI / 6.0f))
            {
                DamageStruct dmg = _projectile.DamageDescriptor;
                dmg.criticalHit = (spellOwner.Emitter as Character).GetCriticalDamage;
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

    }

    public override void OnSetTarget(Entity _ent, Vector3 _position)
    {

    }

    public override void OnValidateTarget(Entity _ent, Vector3 _position, ref bool valid)
    {
        // add visual help to see the back of the target
    }
}
