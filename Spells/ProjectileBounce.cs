using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BounceTarget
{
    ALLY,
    ENEMY,
    ANY
}

public class ProjectileBounce : MonoBehaviour
{
    [SerializeField] BounceTarget bounceTarget = BounceTarget.ANY;
    [SerializeField] int bounceNum = 0;
    [SerializeField] float bounceRange = 0;
    Projectile proj;

    public int BounceNum { get => bounceNum; set => bounceNum = value; }
    public BounceTarget BounceTarget { get => bounceTarget; set => bounceTarget = value; }
    public float BounceRange { get => bounceRange; set => bounceRange = value; }

    private void Awake()
    {
        proj = GetComponent<Projectile>();
    }

    bool IsTrajectoryInterrupted(Entity _entity, Vector3 _position)
    {
        Vector3 targetPosition = ((proj.TargetingMethod & TargetingMethod.POSITION) != TargetingMethod.POSITION) ? _entity.transform.position : _position;
        if (proj.Trajectory == TrajectoryType.INSTANTANEOUS)
        {
            RaycastHit hit;
            return Physics.Raycast(transform.position, (targetPosition - transform.position).normalized, out hit,
                (targetPosition - transform.position).magnitude - 0.01f, (1 << LayerMask.NameToLayer("Environnement")) | (1), QueryTriggerInteraction.Ignore);
        }
        else
        {
            Vector3 lastpos = transform.position;
            Vector3 pos = transform.position;
            float distance = Vector3.Distance(transform.position, targetPosition);
            int stepNum = (int)distance;

            for (int i = 0; i < stepNum; i++)
            {
                RaycastHit hit;
                float progress = i / (float)stepNum;
                lastpos = pos;
                pos = (1.0f - progress) * transform.position + progress * targetPosition
                + Vector3.up * Mathf.Sin(progress * Mathf.PI) * distance * proj.Elevation;

                if (Physics.Raycast(lastpos, (pos - lastpos).normalized, out hit, (pos - lastpos).magnitude, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                {
                    Entity hitEntity = hit.collider.GetComponentInParent<Entity>();
                    float distanceHitTarget = Vector3.Distance(hit.point, targetPosition);
                    if (
                        (proj.Emitter == null || hitEntity != proj.Emitter) &&
                        (((proj.TargetingMethod == TargetingMethod.ENTITY || proj.TargetingMethod == TargetingMethod.CHARACTER) && hitEntity != _entity) ||
                        ((proj.TargetingMethod & TargetingMethod.POSITION) == TargetingMethod.POSITION && distanceHitTarget > 0.5f))
                       )
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }

    bool ValidateTarget(Entity _entity, Vector3 _position)
    {
        if (_entity || (proj.TargetingMethod & TargetingMethod.POSITION) == TargetingMethod.POSITION)
        {
            Vector3 targetPosition = ((proj.TargetingMethod & TargetingMethod.POSITION) == TargetingMethod.POSITION) ? _position : _entity.transform.position;
            if (Vector3.Distance(transform.position, targetPosition) <= BounceRange)
            {
                if (!IsTrajectoryInterrupted(_entity, _position))
                {
                    if ((proj.TargetingMethod & TargetingMethod.POSITION) == TargetingMethod.POSITION)
                    {
                        return true;
                    }
                    else if (!_entity.IsDead)
                    {
                        Character character = (_entity as Character);
                        if (proj.TargetingMethod == TargetingMethod.ENTITY)
                        {
                            return true;
                        }
                        else if (proj.TargetingMethod == TargetingMethod.CHARACTER && character)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    Entity GetNewTarget()
    {
        if ((proj.TargetingMethod & TargetingMethod.CHARACTER) == TargetingMethod.CHARACTER
            || (proj.TargetingMethod & TargetingMethod.ENTITY) == TargetingMethod.ENTITY)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, BounceRange, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            if (colliders != null && colliders.Length > 0)
            {
                int rand = Random.Range(0, colliders.Length);
                for (int i = 0; i < colliders.Length; i++)
                {
                    int index = (i + rand) % colliders.Length;
                    Entity tmpEntity = colliders[index].gameObject.GetComponent<Entity>();
                    Character character = (tmpEntity as Character);
                    if (tmpEntity && tmpEntity != proj.Target && !tmpEntity.IsDead && !Physics.Linecast(tmpEntity.transform.position, transform.position + (tmpEntity.transform.position - transform.position).normalized * 0.01f, (1 << LayerMask.NameToLayer("Environnement")) | (1)))
                    {
                        if (character && ValidateTarget(character, character.transform.position))
                        {
                            if (BounceTarget == BounceTarget.ANY)
                            {
                                return character;
                            }
                            else
                            {
                                bool isAlly = character.Hostility == (proj.Emitter as Character).Hostility || character.Hostility == CHARACTER_HOSTILITY.NEUTRAL;
                                if (isAlly == (BounceTarget == BounceTarget.ALLY))
                                {
                                    return character;
                                }
                            }
                        }
                    }
                }
            }
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (proj.HasHit && BounceNum > 0)
        {
            BounceNum--;
            proj.Target = GetNewTarget();
            if (proj.Target)
            {
                proj.TargetPosition = proj.Target.transform.position;
                proj.Initialise();
            }
        }
    }
}
