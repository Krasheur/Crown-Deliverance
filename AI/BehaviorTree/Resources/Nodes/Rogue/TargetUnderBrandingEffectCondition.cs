using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TargetUnderBrandingEffectCondition : Node
{
    public List<Node> nodes = new List<Node>();

    public override NodeState Evaluate(BlackBoard _bb)
    {
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
        for (int i = 0; i < _bb.Targets.Count; i++)
        {
            if (AmIMarked(_bb.Targets[i]))
            {
                (_bb as BlackBoardRogue).BrandingTarget = _bb.Targets[i];
                return true;
            }
        }

        return false;
    }

    private bool AmIMarked(Entity _entity)
    {
        Alteration[] alList = _entity.GetComponentsInChildren<Alteration>();

        if (alList.Length > 0)
        {
            foreach (Alteration a in alList)
            {
                if (a.DisplayName == "Prey")
                {
                    return true;
                }
            }
        }       

        return false;
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