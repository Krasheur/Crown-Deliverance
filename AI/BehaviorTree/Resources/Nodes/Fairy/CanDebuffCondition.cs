using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CanDebuffCondition : Node
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
        SpellHolder[] shs = _bb.Character.SpellList;

        foreach (SpellHolder sh in shs)
        {
            if (sh.Spell.name != "Heal" && sh.Spell.name != "FireBall" && sh.TurnsRemaining == 0)
            {
                // is a debuff ?
                foreach (SpellAlteration sa in sh.SpellModifiers)
                {
                    if ((sa.Flags & SpellFlagsMask.CanTargetEnemies) == SpellFlagsMask.CanTargetEnemies)
                    {
                        _bb.SpellHolder = sh;
                        return true;
                    }
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
