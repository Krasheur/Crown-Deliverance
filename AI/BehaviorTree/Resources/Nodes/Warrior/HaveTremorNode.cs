using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class HaveTremorNode : Node
{
    public override NodeState Evaluate(BlackBoard _bb)
    {
        if ((_bb as BlackBoardWarrior).TremorCount <= 0)
        {
            SpellHolder[] _shs = _bb.Character.SpellList;

            foreach (SpellHolder sh in _shs)
            {
                if (sh.Spell.name == "EarthTremor" && sh.TurnsRemaining == 0 && _bb.Character.CurrentPa >= sh.Cost)
                {
                    if (AreTargetsAround(_bb, sh.Spell))
                    {
                        _bb.SpellHolder = sh;
                        return NodeState.SUCCESS;
                    }
                }
            }
        }        

        return NodeState.FAILURE;
    }

    bool AreTargetsAround(BlackBoard _bb, Spell _s)
    {
        Collider[] collidersAround = Physics.OverlapSphere(_bb.transform.position, _s.ImpactRadius);

        foreach (Collider c in collidersAround)
        {
            if (c.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }
}
