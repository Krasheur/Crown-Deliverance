using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class BTFairy : BTUser
{
    public override void Launch()
    {
        NodeState current = NodeState.SUCCESS;
        bb.FuturePositions.Clear();

        // Evaluation when last action is done
        if (bb.ActionDone)
        {
            bb.ActionTimer = 20f;
            if (character.NavAgent.enabled) character.NavAgent.ResetPath();
            current = BTManager.main.FairyTree.root.Evaluate(bb);
        }

        // if character is dead or any action to do -> endturn
        if (character.IsDead || current == NodeState.FAILURE)
        {
            bb.Targets.Clear();
            bb.ActionTimer = 20f;
            character.State = CHARACTER_STATE.LOCKED;
            bb.HasHeal = false;
        }

        // if there is action -> do it
        if (bb.Action != null)
        {
            bb.ActionDone = bb.Action();

            // If an action last more than "8 seconds"
            // Consider that's a failure and end turn
            bb.ActionTimer -= Time.deltaTime;
            if (bb.ActionTimer <= 0f)
            {
                bb.ActionDone = true;

                if (character.SpellCasted != null)
                {
                    character.SpellCasted.Cancel();
                }

                character.NavAgent.ResetPath();
                bb.Action = null;
                bb.ActionTimer = 20f;

                character.State = CHARACTER_STATE.LOCKED;
                bb.HasHeal = false;
            }
        }
    }
}
