using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalChanceBonus : SpellAlteration
{
    [SerializeField] int criticalChanceBonus;

    public override string DescriptionShort
    {
        get
        {
            return ("Adds " + criticalChanceBonus + "% to critical hit chance");
        }
    }

    public override void OnFireProjectile(Projectile _projectile)
    {
        Character character = transform.parent.GetComponentInParent<Character>();
        if (character)
        {
            _projectile.CriticalChanceBonus += criticalChanceBonus;
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
