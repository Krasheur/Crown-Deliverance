using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class HaveBuffNode : Node
{
    public override NodeState Evaluate(BlackBoard _bb)
    {
        if ((_bb as BlackBoardWarrior).BuffCount <= 0)
        {
            SpellHolder[] shs = _bb.Character.SpellList;

            foreach (SpellHolder sh in shs)
            {
                if (sh.Spell.name == "Guardian" && sh.TurnsRemaining == 0 && _bb.Character.CurrentPa > sh.Cost)
                {
                    _bb.SpellHolder = sh;
                    return NodeState.SUCCESS;
                }
            }
        }        

        return NodeState.FAILURE;
    }
}
