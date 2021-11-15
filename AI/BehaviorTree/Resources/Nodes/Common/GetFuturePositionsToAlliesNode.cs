using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class GetFuturePositionsToAlliesNode : Node
{
    public override NodeState Evaluate(BlackBoard _bb)
    {
        _bb.FuturePositions.Clear();

        if (_bb.Character.CurrentPa > 0)
        {
            Vector3 destination = Vector3.zero;

            List<Entity> tmp = _bb.Allies;
            float minDist = float.MaxValue;

            for (int i = 0; i < tmp.Count; i++)
            {
                if (Vector3.Distance(_bb.Character.transform.position, tmp[i].transform.position) < minDist)
                {
                    minDist = Vector3.Distance(_bb.Character.transform.position, tmp[i].transform.position);
                    destination = tmp[i].transform.position;
                }
            }

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