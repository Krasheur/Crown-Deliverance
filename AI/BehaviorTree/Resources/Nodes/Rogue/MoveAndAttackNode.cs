using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class MoveAndAttackNode : Node
{
    public override NodeState Evaluate(BlackBoard _bb)
    {
        _bb.Action += (_bb as BlackBoardRogue).MoveAndAttack;
        return NodeState.SUCCESS;
    }
}