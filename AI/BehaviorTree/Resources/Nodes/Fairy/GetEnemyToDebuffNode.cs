using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class GetEnemyToDebuffNode : Node
{
    public override NodeState Evaluate(BlackBoard _bb)
    {
        List<Entity> tmpEnemies = new List<Entity>();

        Collider[] collidersAround = Physics.OverlapSphere(_bb.transform.position, _bb.SpellHolder.Spell.Range + (_bb.Character.CurrentPa - _bb.SpellHolder.Spell.Cost));

        foreach (Collider c in collidersAround)
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

        tmpEnemies = SortEnemiesNumbersOfBuff(tmpEnemies);

        if (tmpEnemies.Count > 0)
        {
            _bb.Targets = tmpEnemies;
            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }

    private List<Entity> SortEnemiesNumbersOfBuff(List<Entity> _ent)
    {
        _ent.Sort(SortFunc);

        return _ent;
    }

    private int SortFunc(Entity _a, Entity _b)
    {
        float numA = _a.GetComponentsInChildren<Alteration>().Length;
        float numB = _b.GetComponentsInChildren<Alteration>().Length;

        return numA.CompareTo(numB);
    }
}
