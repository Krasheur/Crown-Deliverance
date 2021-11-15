using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class GetFuturePositionsToProvocatorNode : Node
{
    public override NodeState Evaluate(BlackBoard _bb)
    {
        _bb.FuturePositions.Clear();

        if (_bb.Character.CurrentPa > 0)
        {
            Vector3 destination = _bb.Character.Target.transform.position - _bb.Character.transform.position;
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
