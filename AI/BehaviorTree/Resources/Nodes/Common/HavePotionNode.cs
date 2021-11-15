using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class HavePotionNode : Node
{
    public override NodeState Evaluate(BlackBoard _bb)
    {
        if (UnderThreshold(_bb) && CheckPotion(_bb))
        {
            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }

    private bool UnderThreshold(BlackBoard _bb)
    {
        if (_bb.Character.PV < _bb.Character.PvMax * 50 / 100)
        {
            return true;
        }

        return false;
    }

    private bool CheckPotion(BlackBoard _bb)
    {
        ItemInventory[] inv = _bb.Character.Inventory;

        for (int i = 0; i < inv.Length; i++)
        {
            if (inv[i].Item as Consumable)
            {
                return true;
            }
        }

        return false;
    }
}
