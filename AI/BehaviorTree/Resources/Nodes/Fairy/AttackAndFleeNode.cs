using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class AttackAndFleeNode : Node
{
    public override NodeState Evaluate(BlackBoard _bb)
    {
        _bb.Action = (_bb as BlackBoardFairy).AttackAndFlee;
        return NodeState.SUCCESS;
    }
}
