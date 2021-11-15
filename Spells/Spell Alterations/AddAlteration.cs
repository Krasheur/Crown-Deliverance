using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddAlteration : SpellAlteration
{
    [SerializeField] Alteration alteration;

    public override string DescriptionShort {
        get 
        {
            if (alteration != null) return (alteration.ApplyingChance + "% chance to apply : " + alteration.DisplayName);
            else return "";
        }
    }
    public override string DescriptionFull {
        get 
        {
            if (alteration != null) return (alteration.ApplyingChance + "% chance to apply : " + alteration.DisplayName + (descriptionFull.Length > 0 ? "\n" + descriptionFull : ""));
            else return "";
        }
    }

    public override void OnFireProjectile(Projectile _projectile)
    {
       if(alteration !=null) _projectile.Alterations.Add(alteration);
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
