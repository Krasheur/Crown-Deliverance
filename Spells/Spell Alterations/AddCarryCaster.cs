using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AddCarryCaster : SpellAlteration
{
    public override void OnFireProjectile(Projectile _projectile)
    {
        if (_projectile.GetComponent<CarryCaster>() == null)
        {
            _projectile.gameObject.AddComponent<CarryCaster>();
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
        NavMeshHit hit;

        if (valid && !NavMesh.SamplePosition(_position, out hit, 1.0f, NavMesh.AllAreas))
        {
            valid = false;
        }
    }
}
