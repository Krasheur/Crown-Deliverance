using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class MoveToFightNode : Node
{
    public override NodeState Evaluate(BlackBoard _bb)
    {
        if (GetTargets(_bb))
        {
            return Move(_bb);
        }

        return NodeState.FAILURE;
    }

    private bool GetTargets(BlackBoard _bb)
    {
        List<Entity> tmpEntities = new List<Entity>();
        Collider[] collidersAround = Physics.OverlapSphere(_bb.transform.position, 50.0f);

        foreach (Collider c in collidersAround)
        {
            if (c.CompareTag("Player"))
            {
                Ray ray = new Ray(_bb.Character.transform.position, c.transform.position - _bb.Character.transform.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 50f))
                {
                    if (hit.transform.GetComponent<Entity>() == c.GetComponent<Entity>())
                    {
                        tmpEntities.Add(c.gameObject.GetComponent<Entity>());
                    }
                }
            }
        }

        if (tmpEntities.Count > 0)
        {
            _bb.Targets = tmpEntities;
            return true;
        }
        return false;
    }

    private NodeState Move(BlackBoard _bb)
    {
        if (_bb.Character.CurrentPa > 0)
        {
            Vector3 destination = Vector3.zero;
            List<Entity> tmp = _bb.Targets;

            for (int i = 0; i < tmp.Count; i++)
            {
                destination += tmp[i].transform.position;
            }

            destination /= tmp.Count;

            if (Vector3.Distance(_bb.Character.transform.position, destination) > 4f)
            {
                _bb.Destination = destination;

                Debug.Log("Move to fight");

                _bb.Action += _bb.Move;
                return NodeState.SUCCESS;
            }            
        }

        return NodeState.FAILURE;
    }
}
