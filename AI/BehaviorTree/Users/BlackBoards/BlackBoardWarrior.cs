using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlackBoardWarrior : BlackBoard
{
    protected Entity entityToKill = null;

    bool attackDone = false;
    int buffCount = 2;
    int tremorCount = 2;

    #region GETTERS/SETTERS

    public Entity EntityToKill { get => entityToKill; set => entityToKill = value; }
    public int BuffCount { get => buffCount; set => buffCount = value; }
    public int TremorCount { get => tremorCount; set => tremorCount = value; }

    #endregion GETTERS/SETTERS

    #region ACTIONS

    public bool Kill()
    {
        Debug.Log("Kill " + spellHolder.Spell.name);

        if (!spell)
        {
            spell = spellHolder.Activate(false);
            character.transform.LookAt(entityToKill.transform.position);
        }

        if (!spell && !attackDone)
        {
            spellHolder.TurnsRemaining = 1;
            action -= Kill;
            return true;
        }

        timer += Time.deltaTime;
        if (!attackDone && timer > 2.5f)
        {
            Entity validTarget = null;
            int invalidTargets = 0;
            for (int i = 0; i < spell.TargetNumberNeeded; i++)
            {
                character.transform.LookAt(entityToKill.transform.position);

                if (!spell.SetTarget(entityToKill, entityToKill.transform.position))
                {
                    if (validTarget != null)
                    {
                        spell.SetTarget(validTarget, validTarget.transform.position);
                    }
                    invalidTargets++;
                }
                else
                {
                    validTarget = entityToKill;
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
            buffCount--;
            TremorCount--;

            timer = 0f;
            attackDone = false;

            action -= Kill;
            return true;
        }

        return false;
    }

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
                buffCount--;
                TremorCount--;

                timer = 0f;
                attackDone = false;

                action -= MoveAndAttack;
                return true;
            }
        }

        return false;
    }

    public bool Buff()
    {
        buffCount = 4;

        Debug.Log("Buff " + spellHolder.Spell.name);

        if (!spell)
        {
            spell = spellHolder.Activate(false);
        }

        if (!spell && !attackDone)
        {
            spellHolder.TurnsRemaining = 1;
            action -= Buff;
            return true;
        }

        timer += Time.deltaTime;
        if (!attackDone && timer > 2.5f)
        {
            spell.SetTarget(character, character.transform.position);

            spell = null;
            attackDone = true;
        }

        if (attackDone && timer > 5f)
        {
            TremorCount--;

            timer = 0f;

            attackDone = false;

            action -= Buff;
            return true;
        }

        return false;
    }

    public bool Tremor()
    {
        TremorCount = 4;

        Debug.Log("Tremor");

        if (!spell)
        {
            spell = spellHolder.Activate(false);
        }

        if (!spell && !attackDone)
        {
            spellHolder.TurnsRemaining = 1;
            action -= Tremor;
            return true;
        }

        timer += Time.deltaTime;
        if (!attackDone && timer > 2.5f)
        {
            spell.SetTarget(null, character.transform.position);

            spell = null;
            attackDone = true;
        }

        if (attackDone && timer > 5f)
        {
            buffCount--;

            timer = 0f;
            attackDone = false;

            action -= Tremor;
            return true;
        }

        return false;
    }

    #endregion ACTIONS
}