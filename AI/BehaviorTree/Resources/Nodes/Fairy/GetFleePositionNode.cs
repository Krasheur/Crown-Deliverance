using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class GetFleePositionNode : Node
{
    public override NodeState Evaluate(BlackBoard _bb)
    {
        if (_bb.Character.CurrentPa > 0)
        {
            // Get the destination (opposite of the average position between all targets)
            Vector3 oppositeDestination = Vector3.zero;
            List<Entity> tmp = _bb.Targets;

            for (int i = 0; i < tmp.Count; i++)
            {
                oppositeDestination += tmp[i].transform.position;
            }

            oppositeDestination /= tmp.Count;

            Vector3 dirToTarget = _bb.Character.transform.position - oppositeDestination;
            Vector3 destination = _bb.Character.transform.position + dirToTarget;
            Vector3 toDestination = destination - _bb.Character.transform.position;

            toDestination = toDestination.normalized * _bb.Character.CurrentPa;
            toDestination += _bb.Character.transform.position;

            _bb.Destination = toDestination;
            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;        
    }
}
