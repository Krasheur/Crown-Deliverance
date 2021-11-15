using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class BTUser : MonoBehaviour
{
    public enum ACTION
    {
        HEAL,
        ATTACK,
        MOVEFORWARD,
        MOVEBACK
    }

    [SerializeField] protected Character character;
    [SerializeField] protected BlackBoard bb;

    private void Start()
    {
        bb.Character = character;
        bb.LastPos = character.transform.position;
    }

    public virtual void Launch()
    {
        
    }
}
