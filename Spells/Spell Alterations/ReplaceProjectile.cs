using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceProjectile : SpellAlteration
{
    [SerializeField] Projectile[] projectiles;
    [SerializeField] TargetingMethod[] targetingMethods;
    TargetingMethod originalTargetingMethod;
    int cursor = 0;
    bool initialised = false;

    public override void OnFireProjectile(Projectile _projectile)
    {
        cursor = (cursor + 1) % projectiles.Length;
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

    private void Update()
    {
        //if (targetingMethods.Length > cursor && targetingMethods[cursor] != TargetingMethod.UNCHANGED)
        //{
        //    spellOwner.TargetingMethod = targetingMethods[cursor];
        //}
        //else
        //{
        //    spellOwner.TargetingMethod = originalTargetingMethod;
        //}
    }

    public override void OnGetProjectile(ref Projectile _projectile)
    {
        if (!initialised)
        {
            initialised = true;
            originalTargetingMethod = spellOwner.TargetingMethod;
        }
        if (projectiles.Length > 0)
        {
            _projectile = projectiles[cursor] ? projectiles[cursor] : _projectile;
            if (targetingMethods.Length > cursor && targetingMethods[cursor] != TargetingMethod.UNCHANGED)
            {
                spellOwner.TargetingMethod = targetingMethods[cursor];
            }
            else
            {
                spellOwner.TargetingMethod = originalTargetingMethod;
            }
        }
    }

    public override void OnGetTargetNeeded(ref int _targNum)
    {
        //_targNum = Mathf.Max(projectiles.Length, _targNum);
    }

    public override void OnSetTarget(Entity _ent, Vector3 _position)
    {
        cursor = (cursor + 1) % projectiles.Length;
        if (spellOwner.TargetNumberCurrent >= spellOwner.TargetNumberNeeded)
        {
            cursor = 0;
        }
    }
}
