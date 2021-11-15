using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CloudSpellDoneCondition : Node
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
        if ((_bb as BlackBoardRogue).IsCloudDone)
        {
            return true;
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
