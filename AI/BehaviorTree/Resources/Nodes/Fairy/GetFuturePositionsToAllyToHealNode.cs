using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class GetFuturePositionsToAllyToHealNode : Node
{
    public override NodeState Evaluate(BlackBoard _bb)
    {
        int entityToSteal = 0;

        if (_bb.Character.CurrentPa > 0)
        {
            Vector3 destination = Vector3.zero;

            for (int i = 0; i < (_bb as BlackBoardFairy).HealSH.Spell.TargetNumberNeeded; i++)
            {
                destination += _bb.Allies[i].transform.position;
            }

            if ((_bb as BlackBoardFairy).IsHealthDrain)
            {
                entityToSteal = 1;
                destination += (_bb as BlackBoardFairy).ToStealHealth.transform.position;
            }

            destination /= ((_bb as BlackBoardFairy).HealSH.Spell.TargetNumberNeeded + entityToSteal);

            destination -= _bb.Character.transform.position;
            Vector3 norm = destination.normalized;

            for (int i = 0; i <= _bb.Character.CurrentPa; i++)
            {
                destination = norm * i + _bb.Character.transform.position;
                _bb.FuturePositions.Add(destination);
            }

            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }
}
