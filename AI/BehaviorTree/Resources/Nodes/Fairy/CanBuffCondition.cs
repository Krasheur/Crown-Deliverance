using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class CanBuffCondition : Node
{
    public List<Node> nodes = new List<Node>();

    private BlackBoard bb;

    int buffOrNot;

    public override NodeState Evaluate(BlackBoard _bb)
    {
        bb = _bb;

        if (Condition(_bb))
        {
            return SubEvaluation(_bb);
        }
        else
        {
            return NodeState.FAILURE;
        }
    }

    public void AddNode(Node _n)
    {
        nodes.Add(_n);
    }

    private bool Condition(BlackBoard _bb)
    {
        SpellHolder[] shs = _bb.Character.SpellList;

        foreach (SpellHolder sh in shs)
        {
            if (sh.Spell.name != "Heal" && sh.Spell.name != "Fire" && sh.TurnsRemaining == 0)
            {
                // teleportation ?
                foreach (SpellAlteration sa in sh.SpellModifiers)
                {
                    if ((sa.Flags & SpellFlagsMask.TeleportEnemy) == SpellFlagsMask.TeleportEnemy)
                    {
                        _bb.SpellHolder = sh;
                        SelectTeleportDestination(_bb, false);
                        return true;
                    }
                    else if ((sa.Flags & SpellFlagsMask.TeleportCaster) == SpellFlagsMask.TeleportCaster)
                    {
                        _bb.SpellHolder = sh;
                        SelectTeleportDestination(_bb, true);
                        return true;
                    }
                }

                // buff ?
                foreach (SpellAlteration sa in sh.SpellModifiers)
                {
                    if ((sa.Flags & SpellFlagsMask.CanTargetAllies) == SpellFlagsMask.CanTargetAllies)
                    {
                        buffOrNot = Random.Range(0, 100);

                        if (buffOrNot > 49)
                        {
                            _bb.SpellHolder = sh;
                            return true;
                        }                        
                    }
                }
            }
        }

        return false;
    }

    private void SelectTeleportDestination(BlackBoard _bb, bool _isSelf)
    {
        (_bb as BlackBoardFairy).TeleportSelf = _isSelf;
        (_bb as BlackBoardFairy).TeleportEnemy = !_isSelf;

        // get flee point as destination ?
        Vector3 destination = Vector3.zero;
        List<Entity> tmpEntities = new List<Entity>();
        Collider[] collidersAround = Physics.OverlapSphere(_bb.transform.position, 20f);

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

        for (int i = 0; i < tmpEntities.Count; i++)
        {
            destination += tmpEntities[i].transform.position;
        }

        destination /= tmpEntities.Count;

        Vector3 dirToTarget = _bb.Character.transform.position - destination;
        Vector3 realDestination = _bb.Character.transform.position + dirToTarget;

        realDestination.y *= 0;

        //NavMeshHit hitn;
        //if (!NavMesh.SamplePosition(realDestination, out hitn, 0f, -1))
        //{
        //    realDestination = _bb.Character.transform.position;
        //}

        (_bb as BlackBoardFairy).TeleportDestination = realDestination;
    }

    private NodeState SubEvaluation(BlackBoard _bb)
    {
        foreach (Node n in nodes)
        {
            switch (n.Evaluate(_bb))
            {
                case NodeState.RUNNING:
                    nodeState = NodeState.RUNNING;
                    return nodeState;
                case NodeState.SUCCESS:
                    nodeState = NodeState.SUCCESS;
                    return nodeState;
                case NodeState.FAILURE:
                    break;
                default:
                    break;
            }
        }

        nodeState = NodeState.FAILURE;
        return NodeState;
    }
}
