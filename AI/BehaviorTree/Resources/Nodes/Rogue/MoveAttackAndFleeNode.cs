using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class MoveAttackAndFleeNode : Node
{
    public override NodeState Evaluate(BlackBoard _bb)
    {
        _bb.Action += (_bb as BlackBoardRogue).MoveAttackAndFlee;
        return NodeState.SUCCESS;
    }
}