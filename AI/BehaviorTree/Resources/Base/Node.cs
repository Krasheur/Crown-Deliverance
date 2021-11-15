using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BehaviorTree
{
    [System.Serializable]
    public class Node
    {
        protected NodeState nodeState;
        public NodeState NodeState { get { return nodeState; } }

        public virtual NodeState Evaluate(BlackBoard _bb)
        {
            return NodeState.FAILURE;
        }
    }

    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }
}