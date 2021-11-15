using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlackBoardRogue : BlackBoard
{
    protected Vector3 fleePoint;

    protected Entity brandingTarget;
    protected SpellHolder cloudSH;
    protected Spell cloudS;
    protected SpellHolder brandSH;
    protected Spell brandS;

    private bool canFlee = false;
    private bool canCloud = false;
    protected bool isCloudDone = false;
    bool isCloudTeleport = false;

    #region GETTERS/SETTERS

    public Vector3 FleePoint { get => fleePoint; set => fleePoint = value; }
    public SpellHolder CloudSH { get => cloudSH; set => cloudSH = value; }
    public Entity BrandingTarget { get => brandingTarget; set => brandingTarget = value; }
    public bool IsCloudDone { get => isCloudDone; set => isCloudDone = value; }
    public SpellHolder BrandSH { get => brandSH; set => brandSH = value; }
    public bool IsCloudTeleport { get => isCloudTeleport; set => isCloudTeleport = value; }

    #endregion GETTERS/SETTERS

    #region ACTIONS

    bool attackDone = false;
    bool attackBisDone = false;
    public bool MoveAndAttack()
    {
        Debug.Log("Move And Attack : " + spellHolder.Spell.name);

        if (character.NavAgent.enabled == false) return false;

        // Move ...
        if (!attackDone)
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
            if (!spell)
            {
                spell = spellHolder.Activate(false);
                character.transform.LookAt(targets[0].transform.position);
            }

            if (!spell && !attackDone)
            {
                spellHolder.TurnsRemaining = 1;
                action -= MoveAndAttack;
                return true;
            }

            timer += Time.deltaTime;
            if (!attackDone && timer > 2.5f)
            {
                Entity validTarget = null;
                int invalidTargets = 0;
                for (int i = 0; i < spell.TargetNumberNeeded; i++)
                {
                    int index = i;

                    if (i >= targets.Count)
                    {
                        index = targets.Count - 1;
                    }

                    character.transform.LookAt(targets[index].transform.position);

                    if (!spell.SetTarget(targets[index], targets[index].transform.position))
                    {
                        if (validTarget != null)
                        {
                            spell.SetTarget(validTarget, validTarget.transform.position);
                        }
                        invalidTargets++;
                    }
                    else
                    {
                        validTarget = targets[index];
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
                attackDone = true;
            }

            if (attackDone && timer > 5f)
            {
                timer = 0f;
                attackDone = false;

                action -= MoveAndAttack;
                return true;
            }
        }

        return false;
    }

    public bool MoveAttackAndFlee()
    {
        Debug.Log("Move, Attack and Flee : " + spellHolder.Spell.name);

        if (!character.NavAgent.enabled) return false;

        // Move ...
        if (!canFlee)
        {
            if (!attackDone)
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

                // ... Attack ...
                if (!spell)
                {
                    spell = spellHolder.Activate(false);
                    character.transform.LookAt(targets[0].transform.position);
                }

                if (!spell && !attackDone)
                {
                    spellHolder.TurnsRemaining = 1;
                    action -= MoveAttackAndFlee;
                    return true;
                }

                timer += Time.deltaTime;
                if (!attackDone && timer > 2.5f)
                {
                    Entity validTarget = null;
                    int invalidTargets = 0;
                    for (int i = 0; i < spell.TargetNumberNeeded; i++)
                    {
                        int index = i;

                        if (i >= targets.Count)
                        {
                            index = targets.Count - 1;
                        }

                        if (!spell.SetTarget(targets[index], targets[index].transform.position))
                        {
                            if (validTarget != null)
                            {
                                spell.SetTarget(validTarget, validTarget.transform.position);
                            }
                            invalidTargets++;
                        }
                        else
                        {
                            character.transform.LookAt(targets[index].transform.position);

                            validTarget = targets[index];
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
                    attackDone = true;
                }

                if (attackDone && timer > 5f)
                {
                    timer = 0f;

                    canFlee = true;
                }
            }
        }
        else
        {
            // ... and Flee
            character.NavAgent.SetDestination(fleePoint);

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

                canFlee = false;
                attackDone = false;
                action -= MoveAttackAndFlee;
                return true;
            }
        }

        return false;
    }

    public bool MoveAttackAndCloud()
    {
        Debug.Log("Move, Attack and Cloud : " + spellHolder.Spell.name);

        if (!character.NavAgent.enabled) return false;

        // Move ...
        if (!canCloud)
        {
            if (!attackDone)
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

                // ... Attack ...
                if (!spell)
                {
                    spell = spellHolder.Activate(false);
                    character.transform.LookAt(targets[0].transform.position);
                }

                if (!spell && !attackDone)
                {
                    spellHolder.TurnsRemaining = 1;
                    action -= MoveAttackAndCloud;
                    return true;
                }

                timer += Time.deltaTime;
                if (!attackDone && timer > 2.5f)
                {
                    Entity validTarget = null;
                    int invalidTargets = 0;
                    for (int i = 0; i < spell.TargetNumberNeeded; i++)
                    {
                        int index = i;

                        if (i >= targets.Count)
                        {
                            index = targets.Count - 1;
                        }

                        if (!spell.SetTarget(targets[index], targets[index].transform.position))
                        {
                            if (validTarget != null)
                            {
                                spell.SetTarget(validTarget, validTarget.transform.position);
                            }
                            invalidTargets++;
                        }
                        else
                        {
                            character.transform.LookAt(targets[index].transform.position);

                            validTarget = targets[index];
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
                    attackDone = true;
                }

                if (attackDone && timer > 5f)
                {
                    timer = 0f;

                    canCloud = true;
                }
            }
        }
        else
        {
            // ... and Cloud
            if (!cloudS)
            {
                cloudS = CloudSH.Activate(false);
            }

            if (!cloudS && !attackBisDone)
            {
                CloudSH.TurnsRemaining = 1;
                action -= MoveAttackAndCloud;
                return true;
            }

            timer += Time.deltaTime;
            if (!attackBisDone && timer > 2.5f)
            {
                if (IsCloudTeleport)
                {
                    if (!cloudS.SetTarget(character, character.transform.position - character.transform.forward * (cloudS.Range - 1)))
                    {
                        spellHolder.TurnsRemaining = 1;
                        character.SpellCasted.Cancel();

                        timer = 0f;

                        canCloud = false;
                        isCloudDone = true;
                        attackDone = false;
                        attackBisDone = false;
                        isCloudTeleport = false;

                        action -= MoveAttackAndCloud;
                        return true;
                    }
                }
                else
                {
                    if (!cloudS.SetTarget(character, character.transform.position))
                    {
                        spellHolder.TurnsRemaining = 1;
                        character.SpellCasted.Cancel();

                        timer = 0f;

                        canCloud = false;
                        isCloudDone = true;
                        attackDone = false;
                        attackBisDone = false;
                        isCloudTeleport = false;

                        action -= MoveAttackAndCloud;
                        return true;
                    }
                }

                cloudS = null;
                attackBisDone = true;
            }

            if (attackBisDone && timer > 5f)
            {
                timer = 0f;

                canCloud = false;
                isCloudDone = true;
                attackDone = false;
                attackBisDone = false;
                isCloudTeleport = false;

                action -= MoveAttackAndCloud;
                return true;
            }
        }

        return false;
    }

    public bool ApplyBrandingEffect()
    {
        Debug.Log("Apply Branding Effect");

        if (character.NavAgent.enabled == false) return false;

        if (!brandS)
        {
            brandS = brandSH.Activate(false);
            character.transform.LookAt(targets[0].transform.position);
        }

        if (!brandS && !attackDone)
        {
            brandSH.TurnsRemaining = 1;
            action -= ApplyBrandingEffect;
            return true;
        }

        timer += Time.deltaTime;
        if (!attackDone && timer > 2.5f)
        {
            Entity validTarget = null;
            int invalidTargets = 0;
            for (int i = 0; i < brandS.TargetNumberNeeded; i++)
            {
                int index = i;

                if (i >= targets.Count)
                {
                    index = targets.Count - 1;
                }

                if (!brandS.SetTarget(targets[index], targets[index].transform.position))
                {
                    if (validTarget != null)
                    {
                        brandS.SetTarget(validTarget, validTarget.transform.position);
                    }
                    invalidTargets++;
                }
                else
                {
                    character.transform.LookAt(targets[index].transform.position);

                    validTarget = targets[index];
                }
            }

            if (invalidTargets == brandS.TargetNumberNeeded)
            {
                brandSH.TurnsRemaining = 1;
                character.SpellCasted.Cancel();

                //Debug.Log("AI : Attack Cancel");
            }
            else
            {
                //Debug.Log("AI : Attack Done");
            }

            brandS = null;
            attackDone = true;
        }

        if (attackDone && timer > 5f)
        {
            timer = 0f;

            attackDone = false;

            action -= ApplyBrandingEffect;
            return true;
        }

        return false;
    }

    public bool Flee()
    {
        Debug.Log("Flee");

        if (character.NavAgent.enabled == false) return false;

        character.NavAgent.SetDestination(fleePoint);
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

            action -= Flee;
            return true;
        }

        return false;
    }

    #endregion ACTIONS
}