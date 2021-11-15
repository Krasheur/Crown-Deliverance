using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class GetAllyToBuffNode : Node
{
    BlackBoard bb;

    public override NodeState Evaluate(BlackBoard _bb)
    {
        bb = _bb;

        List<Entity> tmpEnemies = new List<Entity>();

        // Look for enemies if need to teleport one
        if ((_bb as BlackBoardFairy).TeleportEnemy)
        {
            Collider[] enemiesAround = Physics.OverlapSphere(_bb.transform.position, _bb.SpellHolder.Spell.Range + (_bb.Character.CurrentPa - _bb.SpellHolder.Spell.Cost));

            foreach (Collider c in enemiesAround)
            {
                if (c.CompareTag("Player") && c.GetComponent<Character>().Hostility == CHARACTER_HOSTILITY.ALLY)
                {
                    Ray ray = new Ray(_bb.Character.transform.position, c.transform.position - _bb.Character.transform.position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 50f))
                    {
                        if (hit.transform.GetComponent<Entity>() == c.GetComponent<Entity>())
                        {
                            tmpEnemies.Add(c.gameObject.GetComponent<Entity>());
                        }
                    }
                }
            }

            tmpEnemies = SortEnemiesByDistance(tmpEnemies);

            if (tmpEnemies.Count == 0)
            {
                return NodeState.FAILURE;
            }

            _bb.Targets = tmpEnemies;
        }


        List<Entity> tmpEntities = new List<Entity>();
        tmpEntities.Add(_bb.Character);

        Collider[] collidersAround = Physics.OverlapSphere(_bb.transform.position, _bb.SpellHolder.Spell.Range + (_bb.Character.CurrentPa - _bb.SpellHolder.Spell.Cost));

        foreach (Collider c in collidersAround)
        {
            if (c.CompareTag("Character") && c.GetComponent<Character>().Hostility == CHARACTER_HOSTILITY.ENEMY)
            {
                Ray ray = new Ray(_bb.Character.transform.position, c.transform.position - _bb.Character.transform.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 50f))
                {
                    if (hit.transform.GetComponent<Entity>() == c.GetComponent<Entity>())
                    {
                        tmpEntities.Add(c.gameObject.GetComponent<Entity>());
                    }
                }
            }
        }

        if (tmpEntities.Count > 0)
        {
            tmpEntities = SortAlliesNumbersOfBuff(tmpEntities);
            _bb.Allies = tmpEntities;
            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }

    private List<Entity> SortAlliesNumbersOfBuff(List<Entity> _ent)
    {
        _ent.Sort(SortAlliesFunc);

        return _ent;
    }

    private int SortAlliesFunc(Entity _a, Entity _b)
    {
        float numA = _a.GetComponentsInChildren<Alteration>().Length;
        float numB = _b.GetComponentsInChildren<Alteration>().Length;

        return numA.CompareTo(numB);
    }

    private List<Entity> SortEnemiesByDistance(List<Entity> _ent)
    {
        _ent.Sort(SortEnemiesFunc);

        return _ent;
    }

    private int SortEnemiesFunc(Entity _a, Entity _b)
    {
        float distA = Vector3.Distance(_a.transform.position, bb.Character.transform.position);
        float distB = Vector3.Distance(_b.transform.position, bb.Character.transform.position);

        return distA.CompareTo(distB);
    }
}
