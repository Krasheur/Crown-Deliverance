using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using BehaviorTree;

public enum AI_STATE
{
    INACTIVE,
    ANALYSIS,
    ATTACK,
    MOVE_FORWARD,
    MOVE_BACK
}

public class AIController : Controller
{
    [SerializeField] BTUser BT = null;
    [SerializeField] List<Transform> waypoints = new List<Transform>();

    [SerializeField] FOVDetection fov = null;

    float timer = 1.0f;
    bool onMove = false;
    int currentWayPoint = 0;
    bool onPatrol = true;
    bool isOnAlert = false;
    public Entity nearEntity = null;
    float patrolTimer = 0f;

    private void Start()
    {
        timer = Random.Range(1.0f, 5.0f);
    }

    #region CONTROLLER OVERRIDES
    protected override void HandleFreeInput()
    {
        fov.enabled = true;

        if (character.IsDead)
        {
            if (character.SpellCasted != null)
            {
                character.SpellCasted.Cancel();
            }

            this.enabled = false;
            fov.enabled = false;
        }
        else
        {
            if (onPatrol)
            {
                patrolTimer += Time.deltaTime; 
                Patrol();

                character.OnTakeDamage += GetDamageStruct;
            }
            else if (isOnAlert)
            {
                OnAlert();
            }
        }
    }

    protected override void HandleFightInput()
    {
        fov.enabled = false;

        if (character.IsDead)
        {
            if (character.SpellCasted != null)
            {
                character.SpellCasted.Cancel();
            }

            this.enabled = false;
            fov.enabled = false;
        }
        else
        {

            if (onMove)
            {
                if (character.NavAgent.enabled) character.NavAgent.ResetPath();
                onMove = false;
            }

            if (onPatrol)
            {
                if (character.NavAgent.enabled) character.NavAgent.ResetPath();
                onPatrol = false;
            }

            if (isOnAlert)
            {
                if (character.NavAgent.enabled) character.NavAgent.ResetPath();
                isOnAlert = false;
            }

            switch (character.Classe)
            {
                case CLASSES.ASSASSIN:
                    BT.Launch();
                    break;

                case CLASSES.TANK:
                    BT.Launch();
                    break;

                case CLASSES.WIZARD:
                    BT.Launch();
                    break;

                default:
                    BT.Launch();
                    break;
            }
        }
    }

    protected override void HandleLockInput()
    {
        fov.enabled = false;

        if (character.IsDead)
        {
            if (character.SpellCasted != null)
            {
                character.SpellCasted.Cancel();
            }
            
            this.enabled = false;
            fov.enabled = false;
        }
    }
    #endregion CONTROLLER OVERRIDES

    void Patrol()
    {
        if (waypoints.Count > 0)
        {
            if (character.NavAgent.remainingDistance < 0.01f)
            {
                timer -= Time.deltaTime;

                if (timer <= 0f)
                {
                    currentWayPoint++;
                    if (currentWayPoint == waypoints.Count)
                    {
                        currentWayPoint = 0;
                    }

                    timer = Random.Range(1.0f, 5.0f);
                }
            }

            if (waypoints.Count > 1)
            {
                character.NavAgent.SetDestination(waypoints[currentWayPoint].position);
            }
        }
        else
        {
            timer -= Time.deltaTime;

            if (onMove)
            {
                if (character.NavAgent.remainingDistance < 0.01f)
                {
                    onMove = false;
                    timer = Random.Range(1.0f, 5.0f);
                }
            }
            else
            {
                if (timer <= 0f)
                {
                    Vector2 randomPoint = Random.insideUnitCircle * 8.0f;
                    Vector3 randomDestination = character.transform.position + new Vector3(randomPoint.x, 0f, randomPoint.y);
                    character.NavAgent.SetDestination(randomDestination);

                    onMove = true;
                }
            }
        }

        directionLeft = Quaternion.AngleAxis(fov.Angle + 15f, character.transform.up) * character.transform.forward * fov.Radius;
        directionRight = Quaternion.AngleAxis(-fov.Angle - 15f, character.transform.up) * character.transform.forward * fov.Radius;

        // Discuss with a friend if there is one in the FOV and after at least 10s of patrol
        //if (patrolTimer > 10f && fov.isFriendInFOV(7f))
        //{
        //    character.NavAgent.SetDestination(fov.target.transform.position);

        //    if (character.NavAgent.remainingDistance < 3f)
        //    {
        //        character.NavAgent.ResetPath();
        //        (fov.target as Character).NavAgent.ResetPath();

        //        onPatrol = false;
        //        onDiscuss = true;
        //        character.transform.LookAt(fov.target.transform);

        //        AIController ac = (fov.target as Character).transform.GetChild(1).GetComponent<AIController>();
        //        ac.onPatrol = false;
        //        ac.onDiscuss = true;
        //        fov.target.transform.LookAt(character.transform);

        //        patrolTimer = 0f;
        //        ac.patrolTimer = 0f;
        //    }
        //}
    }

    float onLookAroundTimer = 4f;
    float onAlertTimer = 20f;
    Vector3 directionLeft;
    Vector3 directionRight;
    bool entry = true;
    float entryTimer = 0f;
    bool alertMove = false;
    void OnAlert()
    {
        fov.Angle = 60f;
        fov.Radius = 15f;

        if (fov.isTargetInFOV(15f))
        {
            character.NavAgent.SetDestination(fov.target.transform.position);

            onLookAroundTimer = 4f;
            onAlertTimer = 20f;
        }
        else
        {
            if (!alertMove)
            {
                onLookAroundTimer -= Time.deltaTime;
                onAlertTimer -= Time.deltaTime;

                // Look around
                directionLeft.y *= 0;
                directionRight.y *= 0;

                if (entry)
                {
                    entryTimer += Time.deltaTime / 10f;
                    character.transform.forward = Vector3.Lerp(character.transform.forward, Vector3.Lerp(directionRight.normalized, directionLeft.normalized, MathFunc(Time.time % 3)), entryTimer);

                    if (entryTimer >= 1)
                    {
                        entry = false;
                        entryTimer = 0f;
                    }
                }
                else
                {
                    character.transform.forward = Vector3.Lerp(directionRight.normalized, directionLeft.normalized, MathFunc(Time.time % 3));
                }
            }            

            // End of alert after 10s if any threat found
            if (onLookAroundTimer <= 0f)
            {
                if (!alertMove)
                {
                    Vector2 randomPoint = Random.insideUnitCircle * 4.0f;
                    Vector3 randomDestination = character.transform.position + new Vector3(randomPoint.x, 0f, randomPoint.y);
                    character.NavAgent.SetDestination(randomDestination);
                    alertMove = true;
                }                

                directionLeft = Quaternion.AngleAxis(fov.Angle + 15f, character.transform.up) * character.transform.forward * fov.Radius;
                directionRight = Quaternion.AngleAxis(-fov.Angle - 15f, character.transform.up) * character.transform.forward * fov.Radius;

                

                if (character.NavAgent.remainingDistance < 0.1f)
                {
                    character.NavAgent.ResetPath();
                    onLookAroundTimer = 4f;
                    alertMove = false;
                }
            }

            if (onAlertTimer <= 0f)
            {
                fov.Angle = 45f;
                fov.Radius = 6f;

                character.NavAgent.ResetPath();

                onPatrol = true;
                alertMove = false;
            }
        }        
    }

    public void GetDamageStruct(Entity _ent, ref DamageStruct _dm)
    {
        if (character.NavAgent.enabled) character.NavAgent.ResetPath();
        onPatrol = false;
        isOnAlert = true;
        character.OnTakeDamage -= GetDamageStruct;
    }

    private float MathFunc(float _x)
    {
        if (_x >= 0 && _x < 1)
        {
            return 0f;
        }

        if (_x >= 1 && _x < 1.5)
        {
            return 2 * _x - 2;
        }

        if (_x >= 1.5 && _x < 2.5)
        {
            return 1f;
        }

        if (_x >= 2.5 && _x <= 3)
        {
            return - 2 * _x + 6;
        }

        return -1;
    }
}