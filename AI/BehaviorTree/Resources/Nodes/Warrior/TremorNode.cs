using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TremorNode : Node
{
    public override NodeState Evaluate(BlackBoard _bb)
    {
        _bb.Action += (_bb as BlackBoardWarrior).Tremor;
        return NodeState.SUCCESS;
    }
}
