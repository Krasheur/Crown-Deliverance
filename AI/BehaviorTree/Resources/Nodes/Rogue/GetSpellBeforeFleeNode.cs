using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class GetSpellBeforeFleeNode : Node
{
    public override NodeState Evaluate(BlackBoard _bb)
    {
        SpellHolder[] tmp = _bb.Character.SpellList;

        SpellHolder tmpSpellSelected = null;
        Vector3 tmpDestination = Vector3.zero;

        for (int i = _bb.FuturePositions.Count - 1; i >= 0; i--)
        {
            for (int j = 0; j < tmp.Length; j++)
            {
                if (tmp[j].Spell.name != "PoisonCloud" && tmp[j].Spell.name != "ShadowMark" && IsSpellValid(tmp[j], _bb.FuturePositions[i], i + 1, _bb))
                {
                    tmpSpellSelected = tmp[j];
                    tmpDestination = _bb.FuturePositions[i];
                }
            }
        }

        if (tmpSpellSelected != null)
        {
            _bb.SpellHolder = tmpSpellSelected;
            _bb.Destination = tmpDestination;
            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }

    private bool IsSpellValid(SpellHolder _sh, Vector3 _v, int _pa, BlackBoard _bb)
    {
        Spell spell = _sh.Spell;

        if (_bb.Character.CurrentPa - _pa >= spell.Cost && _sh.TurnsRemaining == 0)
        {
            if (_bb.Targets.Count >= spell.TargetNumberNeeded)
            {
                int nbValidTargets = 0;
                for (int i = 0; i < _bb.Targets.Count; i++)
                {
                    if (Vector3.Distance(_v, _bb.Targets[i].transform.position) <= spell.Range)
                    {
                        nbValidTargets++;
                    }
                }

                if (nbValidTargets >= spell.TargetNumberNeeded)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
