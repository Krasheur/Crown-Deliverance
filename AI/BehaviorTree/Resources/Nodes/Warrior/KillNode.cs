using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class KillNode : Node
{
    public override NodeState Evaluate(BlackBoard _bb)
    {
        _bb.Action += (_bb as BlackBoardWarrior).Kill;
        return NodeState.SUCCESS;
    }
}