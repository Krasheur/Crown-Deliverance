using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class AttackNode : Node
{
    public override NodeState Evaluate(BlackBoard _bb)
    {
        _bb.Action += (_bb as BlackBoardWarrior).MoveAndAttack;
        return NodeState.SUCCESS;
    }
}