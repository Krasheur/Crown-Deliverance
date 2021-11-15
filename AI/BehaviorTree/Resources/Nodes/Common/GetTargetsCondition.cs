using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class GetTargetsCondition : Node
{
    public List<Node> nodes = new List<Node>();

    private BlackBoard bb;

    public override NodeState Evaluate(BlackBoard _bb)
    {
        bb = _bb;

        if (Condition(_bb))
        {
            return SubEvaluation(_bb);
        }
        else
        {
            return NodeState.FAILURE;
        }
    }

    public void AddNode(Node _n)
    {
        nodes.Add(_n);
    }

    private bool Condition(BlackBoard _bb)
    {
        SpellHolder sh = GetSpellWithLargestRange(_bb);
        List<Entity> tmpEntities = new List<Entity>();
        Collider[] collidersAround = Physics.OverlapSphere(_bb.transform.position, sh.Spell.Range + (_bb.Character.CurrentPa - sh.Spell.Cost));

        foreach (Collider c in collidersAround)
        {
            if (c.CompareTag("Player"))
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
            tmpEntities = SortEnemiesByDistance(tmpEntities);
            _bb.Targets = tmpEntities;
            return true;
        }

        return false;
    }

    private SpellHolder GetSpellWithLargestRange(BlackBoard _bb)
    {
        SpellHolder[] tmpSh = _bb.Character.SpellList;

        float rangeMax = float.MinValue;
        SpellHolder sh = null;

        for (int i = 0; i < tmpSh.Length; i++)
        {
            if (tmpSh[i].Spell.Range > rangeMax)
            {
                rangeMax = tmpSh[i].Spell.Range;
                sh = tmpSh[i];
            }
        }

        return sh;
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

    private NodeState SubEvaluation(BlackBoard _bb)
    {
        foreach (Node n in nodes)
        {
            switch (n.Evaluate(_bb))
            {
                case NodeState.RUNNING:
                    nodeState = NodeState.RUNNING;
                    return nodeState;
                case NodeState.SUCCESS:
                    nodeState = NodeState.SUCCESS;
                    return nodeState;
                case NodeState.FAILURE:
                    break;
                default:
                    break;
            }
        }

        nodeState = NodeState.FAILURE;
        return NodeState;
    }
}