﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class MoveAndDebuffNode : Node
{
    public override NodeState Evaluate(BlackBoard _bb)
    {
        _bb.Action += (_bb as BlackBoardFairy).MoveAndDebuff;
        return NodeState.SUCCESS;
    }
}
