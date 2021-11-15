using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class ProvocationAttackNode : Node
{
    public override NodeState Evaluate(BlackBoard _bb)
    {
        _bb.Action += _bb.ProvocationAttack;
        return NodeState.SUCCESS;
    }
}
