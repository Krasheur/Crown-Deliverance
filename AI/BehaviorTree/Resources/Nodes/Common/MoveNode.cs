using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class MoveNode : Node
{
    public override NodeState Evaluate(BlackBoard _bb)
    {
        _bb.Action += _bb.Move;
        return NodeState.SUCCESS;
    }
}