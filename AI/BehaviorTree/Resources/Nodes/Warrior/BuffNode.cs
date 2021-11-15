using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class BuffNode : Node
{
    public override NodeState Evaluate(BlackBoard _bb)
    {
        _bb.Action += (_bb as BlackBoardWarrior).Buff;
        return NodeState.SUCCESS;
    }
}
