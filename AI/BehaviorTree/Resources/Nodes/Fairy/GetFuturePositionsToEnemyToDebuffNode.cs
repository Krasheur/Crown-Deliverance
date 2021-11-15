using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class GetFuturePositionsToEnemyToDebuffNode : Node
{
    public override NodeState Evaluate(BlackBoard _bb)
    {
        if (_bb.Character.CurrentPa > 0)
        {
            Vector3 destination = Vector3.zero;

            for (int i = 0; i < _bb.SpellHolder.Spell.TargetNumberNeeded; i++)
            {
                destination += _bb.Targets[i].transform.position;
            }

            destination /= _bb.SpellHolder.Spell.TargetNumberNeeded;

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
