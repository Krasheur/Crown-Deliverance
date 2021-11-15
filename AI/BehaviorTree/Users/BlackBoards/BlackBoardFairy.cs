using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlackBoardFairy : BlackBoard
{
    protected SpellHolder healSH = null;
    private Spell healS = null;
    protected SpellHolder fireSH = null;
    private Spell fireS = null;

    private bool teleportSelf = false;
    private bool teleportEnemy = false;
    private Vector3 teleportDestination;

    private bool isHealthDrain = false;
    private Entity toStealHealth;

    #region GETTERS/SETTERS

    public SpellHolder HealSH { get => healSH; set => healSH = value; }
    public SpellHolder FireSH { get => fireSH; set => fireSH = value; }
    public bool TeleportSelf { get => teleportSelf; set => teleportSelf = value; }
    public Vector3 TeleportDestination { get => teleportDestination; set => teleportDestination = value; }
    public bool TeleportEnemy { get => teleportEnemy; set => teleportEnemy = value; }
    public bool IsHealthDrain { get => isHealthDrain; set => isHealthDrain = value; }
    public Entity ToStealHealth { get => toStealHealth; set => toStealHealth = value; }

    #endregion GETTERS/SETTERS

    #region ACTIONS

    bool healDone = false;
    public bool MoveAndHeal()
    {
        Debug.Log("Move and Heal");

        if (character.NavAgent.enabled == false) return false;

        // Move ...
        character.NavAgent.SetDestination(destination);

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

            // ... and Heal
            if (isHealthDrain)
            {
                if (!healS)
                {
                    healS = healSH.Activate(false);
                    character.transform.LookAt(ToStealHealth.transform.position);
                }

                if (!healS && !healDone)
                {
                    healSH.TurnsRemaining = 1;
                    action -= MoveAndHeal;
                    return true;
                }

                timer += Time.deltaTime;
                if (!healDone && timer > 2.5f)
                {
                    Entity validTarget = null;
                    int invalidTargets = 0;
                    for (int i = 0; i < healS.TargetNumberNeeded - 1; i++)
                    {
                        int index = i;

                        if (i >= allies.Count)
                        {
                            index = allies.Count - 1;
                        }

                        character.transform.LookAt(allies[index].transform.position);

                        if (!healS.SetTarget(allies[index], allies[index].transform.position))
                        {
                            if (validTarget != null)
                            {
                                healS.SetTarget(validTarget, validTarget.transform.position);
                            }
                            invalidTargets++;
                        }
                        else
                        {
                            validTarget = allies[index];
                        }
                    }

                    if (invalidTargets == healS.TargetNumberNeeded)
                    {
                        healSH.TurnsRemaining = 1;
                        character.SpellCasted.Cancel();

                        //Debug.Log("AI : Attack Cancel");
                    }
                    else
                    {
                        //Debug.Log("AI : Attack Done");
                    }

                    healS = null;
                    healDone = true;
                }

                if (healDone && timer > 5f)
                {
                    timer = 0f;
                    healDone = false;

                    action -= MoveAndHeal;
                    return true;
                }
            }
            else
            {
                if (!healS)
                {
                    healS = healSH.Activate(false);
                    character.transform.LookAt(allies[0].transform.position);
                }

                if (!healS && !healDone)
                {
                    healSH.TurnsRemaining = 1;
                    action -= MoveAndHeal;
                    return true;
                }

                timer += Time.deltaTime;
                if (!healDone && timer > 2.5f)
                {
                    Entity validTarget = null;
                    int invalidTargets = 0;
                    for (int i = 0; i < healS.TargetNumberNeeded; i++)
                    {
                        int index = i;

                        if (i >= allies.Count)
                        {
                            index = allies.Count - 1;
                        }

                        character.transform.LookAt(allies[index].transform.position);

                        if (!healS.SetTarget(allies[index], allies[index].transform.position))
                        {
                            if (validTarget != null)
                            {
                                healS.SetTarget(validTarget, validTarget.transform.position);
                            }
                            invalidTargets++;
                        }
                        else
                        {
                            validTarget = allies[index];
                        }
                    }

                    if (invalidTargets == healS.TargetNumberNeeded)
                    {
                        healSH.TurnsRemaining = 1;
                        character.SpellCasted.Cancel();

                        //Debug.Log("AI : Attack Cancel");
                    }
                    else
                    {
                        //Debug.Log("AI : Attack Done");
                    }

                    healS = null;
                    healDone = true;
                }

                if (healDone && timer > 5f)
                {
                    timer = 0f;
                    healDone = false;

                    action -= MoveAndHeal;
                    return true;
                }
            }            
        }

        return false;
    }

    bool buffDone = false;
    public bool MoveAndBuff()
    {
        Debug.Log("Move and Buff");

        if (character.NavAgent.enabled == false) return false;

        // Move ...
        if (!buffDone)
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

            // ... and Buff
            if (!spell && !buffDone)
            {
                spell = spellHolder.Activate(false);
                character.transform.LookAt(targets[0].transform.position);
            }

            if (!spell && !buffDone)
            {
                spellHolder.TurnsRemaining = 1;
                action -= MoveAndBuff;
                return true;
            }

            if (teleportSelf)
            {
                timer += Time.deltaTime;
                if (!buffDone && timer > 2.5f)
                {
                    character.transform.LookAt(teleportDestination);

                    spell.SetTarget(null, teleportDestination);
                                      
                    spell = null;
                    buffDone = true;
                }

                if (buffDone && timer > 5f)
                {
                    timer = 0f;
                    buffDone = false;
                    teleportSelf = false;

                    action -= MoveAndBuff;
                    return true;
                }
            }
            else if (teleportEnemy)
            {
                timer += Time.deltaTime;
                if (!buffDone && timer > 2.5f)
                {
                    spell.SetTarget(targets[0], targets[0].transform.position);

                    character.transform.LookAt(teleportDestination);

                    spell.SetTarget(null, teleportDestination);

                    spell = null;
                    buffDone = true;
                }

                if (buffDone && timer > 5f)
                {
                    timer = 0f;
                    buffDone = false;
                    teleportEnemy = false;

                    action -= MoveAndBuff;
                    return true;
                }
            }
            else
            {
                timer += Time.deltaTime;
                if (!buffDone && timer > 2.5f)
                {
                    Entity validTarget = null;
                    int invalidTargets = 0;
                    for (int i = 0; i < spell.TargetNumberNeeded; i++)
                    {
                        int index = i;

                        if (i >= allies.Count)
                        {
                            index = allies.Count - 1;
                        }

                        character.transform.LookAt(allies[index].transform.position);

                        if (!spell.SetTarget(allies[index], allies[index].transform.position))
                        {
                            if (validTarget != null)
                            {
                                spell.SetTarget(validTarget, validTarget.transform.position);
                            }
                            invalidTargets++;
                        }
                        else
                        {
                            validTarget = allies[index];
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
                    buffDone = true;
                }

                if (buffDone && timer > 5f)
                {
                    timer = 0f;
                    buffDone = false;

                    action -= MoveAndBuff;
                    return true;
                }

            }
        }

        return false;
    }

    bool debuffDone = false;
    public bool MoveAndDebuff()
    {
        Debug.Log("Move and Debuff");

        if (character.NavAgent.enabled == false) return false;

        // Move ...
        if (!debuffDone)
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

            // ... and Debuff
            if (!spell)
            {
                spell = spellHolder.Activate(false);
                character.transform.LookAt(targets[0].transform.position);
            }

            if (!spell && !debuffDone)
            {
                spellHolder.TurnsRemaining = 1;
                action -= MoveAndDebuff;
                return true;
            }

            timer += Time.deltaTime;
            if (!debuffDone && timer > 2.5f)
            {
                Entity validTarget = null;
                int invalidTargets = 0;
                for (int i = 0; i < spell.TargetNumberNeeded; i++)
                {
                    character.transform.LookAt(targets[i].transform.position);

                    if (!targets[i] || !spell.SetTarget(targets[i], targets[i].transform.position))
                    {
                        if (validTarget != null)
                        {
                            spell.SetTarget(validTarget, validTarget.transform.position);
                        }
                        invalidTargets++;
                    }
                    else
                    {
                        validTarget = targets[i];
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
                debuffDone = true;
            }

            if (debuffDone && timer > 5f)
            {
                timer = 0f;
                debuffDone = false;

                action -= MoveAndDebuff;
                return true;
            }
        }

        return false;
    }

    bool attackDone = false;
    public bool AttackAndFlee()
    {
        Debug.Log("Attack and Flee");

        if (character.NavAgent.enabled == false) return false;

        // Attack
        timer += Time.deltaTime;
        if (!attackDone)
        {
            if (!fireS)
            {
                fireS = fireSH.Activate(false);
                character.transform.LookAt(targets[0].transform.position);
            }

            if (!fireS && !attackDone)
            {
                fireSH.TurnsRemaining = 1;
                action -= AttackAndFlee;
                return true;
            }

            if (!attackDone && timer > 2.5f)
            {
                Entity validTarget = null;
                int invalidTargets = 0;
                for (int i = 0; i < fireS.TargetNumberNeeded; i++)
                {
                    int index = i;

                    if (i >= targets.Count)
                    {
                        index = targets.Count - 1;
                    }

                    character.transform.LookAt(targets[index].transform.position);

                    if (!fireS.SetTarget(targets[index], targets[index].transform.position))
                    {
                        if (validTarget != null)
                        {
                            fireS.SetTarget(validTarget, validTarget.transform.position);
                        }
                        invalidTargets++;
                    }
                    else
                    {
                        validTarget = targets[index];
                    }
                }

                if (invalidTargets == fireS.TargetNumberNeeded)
                {
                    fireSH.TurnsRemaining = 1;
                    character.SpellCasted.Cancel();

                    //Debug.Log("AI : Attack Cancel");
                }
                else
                {
                    //Debug.Log("AI : Attack Done");
                }

                fireS = null;
                attackDone = true;
            }
        }        

        if (attackDone && timer > 5f)
        {
            timer = 0.0f;

            attackDone = false;
            action -= AttackAndFlee;
            return true;
        }

        return false;
    }

    #endregion ACTIONS
}
