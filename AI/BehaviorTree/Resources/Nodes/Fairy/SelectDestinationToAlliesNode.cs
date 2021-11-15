using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class SelectDestinationToAlliesNode : Node
{
    public override NodeState Evaluate(BlackBoard _bb)
    {
        for (int i = 0; i < _bb.FuturePositions.Count; i++)
        {
            if (CanBuff(i, _bb))
            {
                _bb.Destination = _bb.FuturePositions[i];

                return NodeState.SUCCESS;
            }
        }

        return NodeState.FAILURE;
    }

    private bool CanBuff(int _pa, BlackBoard _bb)
    {
        int valid = 0;

        if (_bb.Character.CurrentPa - _pa > _bb.SpellHolder.Cost)
        {
            for (int i = 0; i < _bb.SpellHolder.Spell.TargetNumberNeeded; i++)
            {
                if (Vector3.Distance(_bb.Character.transform.position, _bb.Allies[i].transform.position) < _bb.SpellHolder.Spell.Range)
                {
                    valid++;
                }
            }
        }

        if (valid == _bb.SpellHolder.Spell.TargetNumberNeeded)
        {
            return true;
        }

        return false;
    }
}
