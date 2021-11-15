using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum RetargetOption
{
    NONE,
    ON_CAST_POSITION,
}

public class TeleportSelf : SpellAlteration
{
    [SerializeField] RetargetOption retargetOption;

    public override void OnFireProjectile(Projectile _projectile)
    {
        if (_projectile.Emitter)
        {
            Vector3 endPosition = _projectile.TargetPosition;
            if (retargetOption == RetargetOption.ON_CAST_POSITION)
            {
                _projectile.TargetPosition = _projectile.Emitter.transform.position;
                _projectile.Target = null;
            }
            _projectile.Emitter.transform.position = endPosition;
            if (_projectile.Emitter as Character)
            {
                (_projectile.Emitter as Character).LastPos = endPosition;
                (_projectile.Emitter as Character).NavAgent.ResetPath();
            }
        }
    }

    public override void OnGetAutomaticHit(ref bool _autHit)
    {

    }

    public override void OnValidateTarget(Entity _ent, Vector3 _position, ref bool valid)
    {
        NavMeshHit hit;
        if (valid && !NavMesh.SamplePosition(_position, out hit, 0.05f, NavMesh.AllAreas))
        {
            valid = false;
        }

        if (retargetOption == RetargetOption.ON_CAST_POSITION)
        {
            spellOwner.AimVisualiser.position = spellOwner.transform.parent.position;
        }
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
