using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class SelectSpellToKillNode : Node
{
    public override NodeState Evaluate(BlackBoard _bb)
    {
        SpellHolder[] tmpSpellList = _bb.Character.SpellList;

        SpellHolder tmpSpellSelected = null;
        Entity tmpTargetSelected = null;

        for (int i = 0; i < tmpSpellList.Length; i++)
        {
            for (int j = 0; j < _bb.Targets.Count; j++)
            {
                if (tmpSpellList[i].TurnsRemaining == 0 && tmpSpellList[i].Spell.Cost < _bb.Character.CurrentPa)
                {
                    if (IsInRange(tmpSpellList[i], _bb.Targets[j], _bb) && IsKilling(tmpSpellList[i], _bb.Targets[j], _bb))
                    {
                        tmpSpellSelected = tmpSpellList[i];
                        tmpTargetSelected = _bb.Targets[j];
                    }
                }                
            }
        }

        if (tmpSpellSelected && tmpTargetSelected)
        {
            _bb.SpellHolder = tmpSpellSelected;
            (_bb as BlackBoardWarrior).EntityToKill = tmpTargetSelected;
            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }

    private bool IsInRange(SpellHolder _sh, Entity _ent, BlackBoard _bb)
    {
        if (_sh.Spell.Range > Vector3.Distance(_bb.Character.transform.position, _ent.transform.position))
        {
            return true;
        }

        return false;
    }

    private bool IsKilling(SpellHolder _sh, Entity _ent, BlackBoard _bb)
    {
        if (_sh.Spell.Damage >= _ent.PV)
        {
            return true;
        }

        return false;
    }
}
