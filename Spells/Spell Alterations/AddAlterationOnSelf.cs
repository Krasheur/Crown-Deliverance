using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddAlterationOnSelf : SpellAlteration
{
    [SerializeField] protected Alteration alteration;

    public override string DescriptionShort {
        get 
        {
            return (alteration.ApplyingChance + "% chance to apply " + alteration.DisplayName + " to the caster"); 
        }
    }
    
    public override string DescriptionFull {
        get 
        {
            return (alteration.ApplyingChance + "% chance to apply " + alteration.DisplayName + " to the caster" + (descriptionFull.Length > 0 ? "\n" + descriptionFull : "")); 
        }
    }

    public override void OnFireProjectile(Projectile _projectile)
    {
        if (_projectile.Emitter)
        {
            Instantiate(alteration, _projectile.Emitter.transform).Emitter = _projectile.Emitter;
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
