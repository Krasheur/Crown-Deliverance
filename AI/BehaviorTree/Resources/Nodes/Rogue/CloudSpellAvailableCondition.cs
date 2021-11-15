using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CloudSpellAvailableCondition : Node
{
    public List<Node> nodes = new List<Node>();

    public override NodeState Evaluate(BlackBoard _bb)
    {
        int randCloud = Random.Range(0, 100);

        if (randCloud > 49 && Condition(_bb))
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
            if (_bb.Character.SpellList[i].Spell.name == "PoisonCloud" && _bb.Character.SpellList[i].TurnsRemaining == 0 && _bb.Character.SpellList[i].Spell.Cost <= _bb.Character.CurrentPa)
            {
                (_bb as BlackBoardRogue).CloudSH = _bb.Character.SpellList[i];

                foreach (SpellAlteration sa in _bb.Character.SpellList[i].SpellModifiers)
                {
                    if ((sa.Flags & SpellFlagsMask.TeleportCaster) == SpellFlagsMask.TeleportCaster)
                    {
                        (_bb as BlackBoardRogue).IsCloudTeleport = true;
                        return true;
                    }
                }

                return true;
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
