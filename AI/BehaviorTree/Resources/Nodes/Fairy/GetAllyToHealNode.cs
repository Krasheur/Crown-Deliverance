using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

// Get the allies who can be heal and select the one who has the less hp

public class GetAllyToHealNode : Node
{
    private BlackBoard bb;

    public override NodeState Evaluate(BlackBoard _bb)
    {
        bb = _bb;

        if ((_bb as BlackBoardFairy).IsHealthDrain)
        {
            GetEntityToStealHealth(_bb);
        }

        List<Entity> tmpAllies = new List<Entity>();

        if (_bb.Character.PV <= _bb.Character.PvMax / 2)
        {
            tmpAllies.Add(_bb.Character);
        }              

        Collider[] collidersAround = Physics.OverlapSphere(_bb.transform.position, (_bb as BlackBoardFairy).HealSH.Spell.Range + (_bb.Character.CurrentPa - (_bb as BlackBoardFairy).HealSH.Spell.Cost));

        foreach (Collider c in collidersAround)
        {
            if (c.CompareTag("Character"))
            {
                Character tmp = c.GetComponent<Character>();

                if (tmp.Hostility == CHARACTER_HOSTILITY.ENEMY && tmp.PV <= tmp.PvMax / 2)
                {
                    Ray ray = new Ray(_bb.Character.transform.position, c.transform.position - _bb.Character.transform.position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 50f))
                    {
                        if (hit.transform.GetComponent<Entity>() == c.GetComponent<Entity>())
                        {
                            tmpAllies.Add(c.gameObject.GetComponent<Entity>());
                        }
                    }
                }                
            }
        }

        tmpAllies = SortAlliesByHP(tmpAllies);

        if (tmpAllies.Count > 0)
        {
            _bb.Allies = tmpAllies;
            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }

    private List<Entity> SortAlliesByHP(List<Entity> _ent)
    {
        _ent.Sort(SortFunc);

        return _ent;
    }

    private int SortFunc(Entity _a, Entity _b)
    {
        float distA = _a.PV;
        float distB = _b.PV;

        return distA.CompareTo(distB);
    }

    private void GetEntityToStealHealth(BlackBoard _bb)
    {
        List<Entity> tmpEntities = new List<Entity>();

        Collider[] collidersAround = Physics.OverlapSphere(_bb.transform.position, (_bb as BlackBoardFairy).HealSH.Spell.Range + (_bb.Character.CurrentPa - (_bb as BlackBoardFairy).HealSH.Spell.Cost));

        foreach (Collider c in collidersAround)
        {
            if (c.CompareTag("Player"))
            {
                Character tmp = c.GetComponent<Character>();

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

        tmpEntities = SortEnemiesByDistance(tmpEntities);

        if (tmpEntities.Count > 0)
        {
            (_bb as BlackBoardFairy).ToStealHealth = tmpEntities[0];
        }
    }

    private List<Entity> SortEnemiesByDistance(List<Entity> _ent)
    {
        _ent.Sort(SortEnemiesFunc);

        return _ent;
    }

    private int SortEnemiesFunc(Entity _a, Entity _b)
    {
        float distA = Vector3.Distance(_a.transform.position, bb.Character.transform.position);
        float distB = Vector3.Distance(_b.transform.position, bb.Character.transform.position);

        return distA.CompareTo(distB);
    }
}
