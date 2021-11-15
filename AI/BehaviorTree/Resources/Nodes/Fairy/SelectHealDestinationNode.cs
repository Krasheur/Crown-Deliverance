using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class SelectHealDestinationNode : Node
{
    public override NodeState Evaluate(BlackBoard _bb)
    {
        for (int i = 0; i < _bb.FuturePositions.Count; i++)
        {
            if (CanHeal(i, _bb))
            {
                _bb.Destination = _bb.FuturePositions[i];

                return NodeState.SUCCESS;
            }
        }

        return NodeState.FAILURE;
    }

    private bool CanHeal(int _pa, BlackBoard _bb)
    {
        int valid = 0;

        if (_bb.Character.CurrentPa - _pa > (_bb as BlackBoardFairy).HealSH.Cost)
        {
            for (int i = 0; i < (_bb as BlackBoardFairy).HealSH.Spell.TargetNumberNeeded; i++)
            {
                if (Vector3.Distance(_bb.Character.transform.position, _bb.Allies[i].transform.position) < (_bb as BlackBoardFairy).HealSH.Spell.Range)
                {
                    valid++;
                }
            }
        }
               
        if (valid == (_bb as BlackBoardFairy).HealSH.Spell.TargetNumberNeeded)
        {
            return true;
        }

        return false;
    }
}
