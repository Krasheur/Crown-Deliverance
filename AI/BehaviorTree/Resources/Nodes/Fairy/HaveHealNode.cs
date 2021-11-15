using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class HaveHealNode : Node
{
    public override NodeState Evaluate(BlackBoard _bb)
    {
        for (int i = 0; i < _bb.Character.SpellList.Length; i++)
        {
            if (_bb.Character.CurrentPa > _bb.Character.SpellList[i].Cost && _bb.Character.SpellList[i].TurnsRemaining == 0 && _bb.Character.SpellList[i].Spell.name == "Heal")
            {
                (_bb as BlackBoardFairy).HealSH = _bb.Character.SpellList[i];

                CheckHealthDrain(_bb, _bb.Character.SpellList[i]);

                return NodeState.SUCCESS;
            }
        }

        return NodeState.FAILURE;
    }

    private void CheckHealthDrain(BlackBoard _bb, SpellHolder _sh)
    {
        foreach (SpellAlteration sa in _sh.SpellModifiers)
        {
            if (((sa.Flags & SpellFlagsMask.DealDamage) == SpellFlagsMask.DealDamage) && ((sa.Flags & SpellFlagsMask.Heal) == SpellFlagsMask.Heal))
            {
                (_bb as BlackBoardFairy).IsHealthDrain = true;
            }
        }
    }
}