using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CanAttackAndFleeCondition : Node
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
        for (int i = 0; i < _bb.Character.SpellList.Length; i++)
        {
            if (_bb.Character.SpellList[i].Spell.name != "Heal")
            {
                if (_bb.Character.CurrentPa > _bb.Character.SpellList[i].Cost && _bb.Character.SpellList[i].TurnsRemaining == 0)
                {
                    if (_bb.Character.SpellList[i].Spell.name == "FireBall")
                    {
                        (_bb as BlackBoardFairy).FireSH = _bb.Character.SpellList[i];
                        return true;
                    }
                    else if (_bb.Character.SpellList[i].Spell.name == "Ice" || _bb.Character.SpellList[i].Spell.name == "Lightning")
                    {
                        if (_bb.Character.SpellList[i].SpellModifiers.Count == 0)
                        {
                            (_bb as BlackBoardFairy).FireSH = _bb.Character.SpellList[i];
                            return true;
                        }                        
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
