using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class DrinkPotionNode : Node
{
    public override NodeState Evaluate(BlackBoard _bb)
    {
        _bb.Action += _bb.DrinkPotion;
        return NodeState.SUCCESS;
    }
}
