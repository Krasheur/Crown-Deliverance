using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum TeleportTarget
{
    SELF,
    ALLY,
    ENEMY,
    ANY
}

public class Teleport : SpellAlteration
{
    [SerializeField] TeleportTarget teleportTarget;
    Entity subject;

    private void Start()
    {
        if (teleportTarget == TeleportTarget.SELF)
        {
            spellOwner.AimVisualiser.position = spellOwner.transform.position;
            spellOwner.ShowTrajectoryPreview = true;
            spellOwner.SetTarget(spellOwner.transform.parent.GetComponent<Entity>(), spellOwner.transform.position);
        }
    }

    public override void OnFireProjectile(Projectile _projectile)
    {
        if (_projectile.Target != subject)
        {
            Vector3 endPosition = _projectile.TargetPosition;
            subject.transform.position = endPosition;
            if (subject as Character)
            {
                (subject as Character).LastPos = endPosition;
                if ((subject as Character).NavAgent.enabled) (subject as Character).NavAgent.ResetPath();
            }
        }
    }

    public override void OnGetAutomaticHit(ref bool _autHit)
    {

    }

    public override void OnValidateTarget(Entity _ent, Vector3 _position, ref bool valid)
    {
        NavMeshHit hit;
        
        if (!subject)
        {
            Character character = _ent as Character;
            Character ownerChar = spellOwner.transform.parent.GetComponent<Character>();
            if (character && teleportTarget != TeleportTarget.SELF)
            {
                if (teleportTarget != TeleportTarget.ANY)
                {
                    bool isAlly = character.Hostility == ownerChar.Hostility || character.Hostility == CHARACTER_HOSTILITY.NEUTRAL;
                    if (isAlly != (teleportTarget == TeleportTarget.ALLY))
                    {
                        valid = false;
                    }
                }
            }
        }
        else if (valid && !NavMesh.SamplePosition(_position, out hit, 0.1f, NavMesh.AllAreas))
        {
            valid = false;
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
        if (!subject) subject = _ent;
    }
}
