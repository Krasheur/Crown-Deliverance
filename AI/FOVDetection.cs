using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVDetection : MonoBehaviour
{
    [SerializeField] Character me;

    public Entity target;
    [SerializeField] float angle;
    [SerializeField] float radius;

    public float Angle { get => angle; set => angle = value; }
    public float Radius { get => radius; set => radius = value; }

    bool IsTargetAvailableForFight()
    {
        Character targetChar = target as Character;
        if (targetChar)
        {
            return targetChar.enabled &&
                targetChar.CurrentFight == null &&
                !targetChar.IsDead;
        }

        return false;
    }

    private void Update()
    {
        if (me.Hostility != CHARACTER_HOSTILITY.NEUTRAL && isTargetInFOV(7f) && IsTargetAvailableForFight())
        {
            me.NavAgent.ResetPath();

            List<Character> fighters = new List<Character>();
            fighters.Add(me);

            me.FindFighter(ref fighters, transform.position);

            Fight fight = FightManager.main.CreateFight();
            fight.SetFighter(fighters);


            FightHUD.main.InitializeTimeline(fight);

            this.enabled = false;
        }
    }

    public bool isTargetInFOV(float _radius)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius);

        foreach(Collider c in colliders)
        {
            if (c.CompareTag("Player"))
            {
                target = c.GetComponent<Entity>();

                Vector3 direction = (target.transform.position - transform.position).normalized;
                direction.y *= 0;

                float directionAngle = Vector3.Angle(transform.forward, direction);

                if (directionAngle <= Angle)
                {
                    Ray ray = new Ray(transform.position, target.transform.position - transform.position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, Radius))
                    {
                        if (hit.transform.GetComponent<Entity>() == target)
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    public bool isFriendInFOV(float _radius)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius);

        foreach (Collider c in colliders)
        {
            if (c.CompareTag("Character") && c.GetComponent<Character>().Hostility == CHARACTER_HOSTILITY.ENEMY)
            {
                target = c.GetComponent<Entity>();

                Vector3 direction = (target.transform.position - transform.position).normalized;
                direction.y *= 0;

                float directionAngle = Vector3.Angle(transform.forward, direction);

                if (directionAngle <= Angle)
                {
                    Ray ray = new Ray(transform.position, target.transform.position - transform.position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, Radius))
                    {
                        if (hit.transform.GetComponent<Entity>() == target)
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    #region DEBUG
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);

        Vector3 fovLine1 = Quaternion.AngleAxis(Angle, transform.up) * transform.forward * radius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-Angle, transform.up) * transform.forward * radius;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, transform.forward * radius);

        if (isTargetInFOV(radius))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, target.transform.position - transform.position);
        }
    }
    #endregion
}