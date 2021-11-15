using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBoard : MonoBehaviour
{
    protected Character character;

    protected List<Entity> targets = new List<Entity>();
    protected List<Entity> allies = new List<Entity>();
    protected SpellHolder spellHolder;
    protected Spell spell;
    protected List<Vector3> futurePositions = new List<Vector3>();
    protected Vector3 destination = Vector3.zero;
    protected bool hasHeal = false;

    public delegate bool ActionsToDo();
    protected ActionsToDo action;

    protected bool actionDone = true;
    protected float timer = 0.0f;
    protected float distanceTraveled = 0f;
    protected Vector3 lastPos;
    protected float actionTimer = 15f;
    protected Entity targetAlly;

    private bool tauntDone = false;

    #region GETTERS/SETTERS

    public Character Character { get => character; set => character = value; }
    public List<Entity> Targets { get => targets; set => targets = value; }
    public List<Entity> Allies { get => allies; set => allies = value; }
    public SpellHolder SpellHolder { get => spellHolder; set => spellHolder = value; }
    public Spell Spell { get => spell; set => spell = value; }
    public List<Vector3> FuturePositions { get => futurePositions; set => futurePositions = value; }
    public Vector3 Destination { get => destination; set => destination = value; }
    public bool HasHeal { get => hasHeal; set => hasHeal = value; }
    public Vector3 LastPos { get => lastPos; set => lastPos = value; }
    public float ActionTimer { get => actionTimer; set => actionTimer = value; }
    public ActionsToDo Action { get => action; set => action = value; }
    public bool ActionDone { get => actionDone; set => actionDone = value; }

    #endregion GETTERS/SETTERS

    #region VIRTUAL ACTIONS

    public virtual bool Move()
    {
        Debug.Log("Move");

        if (character.NavAgent.enabled == false) return false;

        character.NavAgent.SetDestination(destination);
        character.NavAgent.isStopped = false;

        distanceTraveled += Vector3.Distance(character.transform.position, lastPos);
        if (distanceTraveled >= 1f)
        {
            if (character.MobilityBase > 0)
            {
                character.MobilityBase--;
            }
            else
            {
                character.CurrentPa--;
            }
            
            distanceTraveled = 0f;
        }

        lastPos = character.transform.position;

        if (character.NavAgent.pathPending)
        {
            return false;
        }
        else if (character.NavAgent.remainingDistance < 0.1f || character.CurrentPa + character.MobilityBase == 0)
        {
            character.NavAgent.isStopped = true;
            character.NavAgent.ResetPath();

            character.CurrentPa = 0;
            character.MobilityBase = 0;

            action -= Move;
            return true;
        }

        return false;
    }

    public virtual bool DrinkPotion()
    {
        Debug.Log("DrinkPotion");

        if (character.NavAgent.enabled == false) return false;

        action -= DrinkPotion;
        return true;
    }

    public virtual bool ProvocationAttack()
    {
        Debug.Log("Move And Attack (Taunt) : " + spellHolder.Spell.name);

        if (character.NavAgent.enabled == false) return false;

        // Move ...
        if (!tauntDone)
        {
            character.NavAgent.SetDestination(destination);
        }

        distanceTraveled += Vector3.Distance(character.transform.position, lastPos);
        if (distanceTraveled >= 1f)
        {
            if (character.MobilityBase > 0)
            {
                character.MobilityBase--;
            }
            else
            {
                character.CurrentPa--;
            }

            distanceTraveled = 0f;
        }

        lastPos = character.transform.position;

        if (character.NavAgent.pathPending)
        {
            return false;
        }
        else if (character.NavAgent.remainingDistance < 0.1f || character.CurrentPa + character.MobilityBase == 0)
        {
            character.NavAgent.isStopped = true;
            character.NavAgent.ResetPath();

            // ... and Attack
            if (spellHolder)
            {
                if (!spell)
                {
                    spell = spellHolder.Activate(false);
                    character.transform.LookAt(character.Target.transform.position);
                }

                if (!spell && !tauntDone)
                {
                    spellHolder.TurnsRemaining = 1;
                    action -= ProvocationAttack;
                    return true;
                }

                timer += Time.deltaTime;
                if (!tauntDone && timer > 2.5f)
                {
                    Entity validTarget = null;
                    int invalidTargets = 0;
                    for (int i = 0; i < spell.TargetNumberNeeded; i++)
                    {
                        character.transform.LookAt(character.Target.transform.position);

                        if (!spell.SetTarget(character.Target, character.Target.transform.position))
                        {
                            if (validTarget != null)
                            {
                                spell.SetTarget(validTarget, validTarget.transform.position);
                            }
                            invalidTargets++;
                        }
                        else
                        {
                            validTarget = character.Target;
                        }
                    }

                    if (invalidTargets == spell.TargetNumberNeeded)
                    {
                        spellHolder.TurnsRemaining = 1;
                        character.SpellCasted.Cancel();

                        //Debug.Log("AI : Attack Cancel");
                    }
                    else
                    {
                        //Debug.Log("AI : Attack Done");
                    }

                    spell = null;
                    tauntDone = true;
                }

                if (tauntDone && timer > 5f)
                {
                    timer = 0f;
                    tauntDone = false;

                    character.CurrentPa = 0;

                    action -= ProvocationAttack;
                    return true;
                }
            }
            else
            {
                return true;
            }
            
        }

        return false;
    }

    #endregion VIRTUAL ACTONS
}
