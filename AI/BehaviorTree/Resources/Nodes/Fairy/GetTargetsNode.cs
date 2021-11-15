using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class GetTargetsNode : Node
{
    BlackBoard bb;

    public override NodeState Evaluate(BlackBoard _bb)
    {
        bb = _bb;

        List<Entity> tmpEntities = new List<Entity>();
        Collider[] collidersAround = Physics.OverlapSphere(_bb.transform.position, 20f);

        foreach (Collider c in collidersAround)
        {
            if (c.CompareTag("Player"))
            {
                Ray ray = new Ray(_bb.Character.transform.position, c.transform.position - _bb.Character.transform.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 50f))
                {
                    if (hit.transform.GetComponent<Entity>() == c.GetComponent<Entity>() && Vector3.Distance(_bb.Character.transform.position, c.GetComponent<Entity>().transform.position) < (_bb as BlackBoardFairy).FireSH.Spell.Range)
                    {
                        tmpEntities.Add(c.gameObject.GetComponent<Entity>());
                    }
                }
            }
        }

        if (tmpEntities.Count > 0)
        {
            tmpEntities = SortEnemiesByDistance(tmpEntities);
            _bb.Targets = tmpEntities;
            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }

    private List<Entity> SortEnemiesByDistance(List<Entity> _ent)
    {
        _ent.Sort(SortFunc);

        return _ent;
    }

    private int SortFunc(Entity _a, Entity _b)
    {
        float distA = Vector3.Distance(_a.transform.position, bb.Character.transform.position);
        float distB = Vector3.Distance(_b.transform.position, bb.Character.transform.position);

        return distA.CompareTo(distB);
    }
}
