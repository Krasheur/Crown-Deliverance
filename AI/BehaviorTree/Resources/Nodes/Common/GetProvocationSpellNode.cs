using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class GetProvocationSpellNode : Node
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
                if (CheckSpellName(tmp[j].Spell, _bb) && IsSpellValid(tmp[j], _bb.FuturePositions[i], i + 1, _bb))
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
        }
        else
        {
            _bb.Destination = _bb.FuturePositions[_bb.FuturePositions.Count - 1];
        }

        return NodeState.SUCCESS;
    }

    private bool IsSpellValid(SpellHolder _sh, Vector3 _v, int _pa, BlackBoard _bb)
    {
        Spell spell = _sh.Spell;

        if (_bb.Character.CurrentPa - _pa > spell.Cost && _sh.TurnsRemaining == 0)
        {
            if (Vector3.Distance(_v, _bb.Character.Target.transform.position) <= spell.Range)
            {
                return true;
            }
        }

        return false;
    }

    private bool CheckSpellName(Spell _spell, BlackBoard _bb)
    {
        switch (_bb.Character.Classe)
        {
            case CLASSES.WIZARD:

                if (_spell.name != "Heal")
                {
                    return true;
                }

                break;

            case CLASSES.ASSASSIN:

                if (_spell.name != "PoisonCloud" && _spell.name != "ShadowMark")
                {
                    return true;
                }

                break;

            case CLASSES.TANK:

                if (_spell.name != "Guardian")
                {
                    return true;
                }

                break;
        }

        return false;
    }
}
