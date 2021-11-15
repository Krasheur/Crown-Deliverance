using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class MoveAttackAndCloudNode : Node
{
    public override NodeState Evaluate(BlackBoard _bb)
    {
        _bb.Action += (_bb as BlackBoardRogue).MoveAttackAndCloud;
        return NodeState.SUCCESS;
    }
}
