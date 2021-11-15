using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBounces : SpellAlteration
{
    [SerializeField] int bounceNum;
    [SerializeField] float bounceRange;
    [SerializeField] BounceTarget bounceTarget;

    public override string DescriptionFull {
        get 
        {
            return ("Adds " + bounceNum + " bounces on " + bounceTarget.ToString().ToLower() + " characters at a maximum range of " + bounceRange + "m"); 
        }
    }
    
    public override string DescriptionShort {
        get 
        {
            return ("Bounces " + bounceNum + " times on " + bounceTarget.ToString().ToLower() + " characters"); 
        }
    }

    public override void OnFireProjectile(Projectile _projectile)
    {
        ProjectileBounce bounce = _projectile.GetComponent<ProjectileBounce>();
        if (!bounce) bounce = _projectile.gameObject.AddComponent<ProjectileBounce>();

        bounce.BounceNum += bounceNum;
        bounce.BounceRange = bounceRange;
        bounce.BounceTarget = bounceTarget;
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
