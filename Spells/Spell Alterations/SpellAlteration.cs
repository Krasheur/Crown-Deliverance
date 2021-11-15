using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellAlteration : MonoBehaviour
{
    [SerializeField] protected string displayName;
    [SerializeField] protected string descriptionFull;
    [SerializeField] protected string descriptionShort;
    [SerializeField] [EnumFlag] SpellFlagsMask flags;
    protected Spell spellOwner;

    public virtual string DescriptionFull { get => descriptionFull; }
    public virtual string DescriptionShort { get => descriptionShort; }
    public string DisplayName { get => displayName; set => displayName = value; }
    public SpellFlagsMask Flags { get => flags; set => flags = value; }

    public abstract void OnFireProjectile(Projectile _projectile);
    public abstract void OnGetDamageAmount(ref int _dmg);
    public abstract void OnGetHealAmount(ref int _hl);
    public abstract void OnGetImpactRadius(ref float _rad);
    public abstract void OnGetIgnoreArmor(ref bool _ignArm);
    public abstract void OnGetAutomaticHit(ref bool _autHit);
    public abstract void OnGetProjectile(ref Projectile _projectile);
    public abstract void OnGetTargetNeeded(ref int _targNum);
    public abstract void OnSetTarget(Entity _ent, Vector3 _position);
    public virtual void OnValidateTarget(Entity _ent, Vector3 _position, ref bool valid)
    {

    }


    private void Awake()
    {
        spellOwner = transform.parent.GetComponent<Spell>();
        if (spellOwner)
        {
            spellOwner.OnFireProjectile += OnFireProjectile;
            spellOwner.OnGetDamageAmount += OnGetDamageAmount;
            spellOwner.OnGetHealAmount += OnGetHealAmount;
            spellOwner.OnGetIgnoreArmor += OnGetIgnoreArmor;
            spellOwner.OnGetAutomaticHit += OnGetAutomaticHit;
            spellOwner.OnGetProjectile += OnGetProjectile;
            spellOwner.OnGetTargetNeeded += OnGetTargetNeeded;
            spellOwner.OnSetTarget += OnSetTarget;
            spellOwner.OnGetImpactRadius += OnGetImpactRadius;
            spellOwner.OnValidateTarget += OnValidateTarget;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected void OnDestroy()
    {
        if (spellOwner)
        {
            spellOwner.OnFireProjectile -= OnFireProjectile;
            spellOwner.OnGetDamageAmount -= OnGetDamageAmount;
            spellOwner.OnGetHealAmount -= OnGetHealAmount;
            spellOwner.OnGetIgnoreArmor -= OnGetIgnoreArmor;
            spellOwner.OnGetAutomaticHit -= OnGetAutomaticHit;
            spellOwner.OnGetProjectile -= OnGetProjectile;
            spellOwner.OnGetTargetNeeded -= OnGetTargetNeeded;
            spellOwner.OnSetTarget -= OnSetTarget;
            spellOwner.OnGetImpactRadius -= OnGetImpactRadius;
            spellOwner.OnValidateTarget -= OnValidateTarget;
            spellOwner = null;
        }
    }
}
