using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class ApplyBrandingEffectNode : Node
{
    public override NodeState Evaluate(BlackBoard _bb)
    {
        for (int i = 0; i < _bb.Character.SpellList.Length; i++)
        {
            if (_bb.Character.SpellList[i].Spell.name == "ShadowMark" && _bb.Character.SpellList[i].TurnsRemaining == 0 && _bb.Character.SpellList[i].Cost <= _bb.Character.CurrentPa)
            {
                (_bb as BlackBoardRogue).BrandSH = _bb.Character.SpellList[i];
                _bb.Action += (_bb as BlackBoardRogue).ApplyBrandingEffect;
                return NodeState.SUCCESS;
            }
        }

        return NodeState.FAILURE;
    }
}
