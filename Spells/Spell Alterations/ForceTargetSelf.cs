using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceTargetSelf : SpellAlteration
{
    bool done = false;
    public override void OnFireProjectile(Projectile _projectile)
    {

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

    public override void OnValidateTarget(Entity _ent, Vector3 _position, ref bool valid)
    {
        Character emitter = spellOwner.transform.parent ? spellOwner.transform.parent.GetComponent<Character>() : null;
        if (emitter)
        {
            valid = true;
            spellOwner.AimVisualiser.position = emitter.transform.position;
            spellOwner.ShowTrajectoryPreview = true;
            if ((spellOwner.TargetNumberNeeded > 1 || Input.GetMouseButtonDown(0)) && !done)
            {
                done = true;
                spellOwner.SetTarget(emitter, emitter.transform.position);
                OnDestroy();
            }
        }
    }

    public override void OnSetTarget(Entity _ent, Vector3 _position)
    {

    }
}
