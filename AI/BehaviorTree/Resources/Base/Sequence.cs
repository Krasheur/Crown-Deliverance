using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using BehaviorTree;

namespace BehaviorTree
{
    [System.Serializable]
    public class Sequence : Node
    {
        public List<Node> nodes = new List<Node>();

        public override NodeState Evaluate(BlackBoard _bb)
        {
            bool isAnyNodeRunning = false;

            foreach (Node n in nodes)
            {
                switch (n.Evaluate(_bb))
                {
                    case NodeState.RUNNING:
                        isAnyNodeRunning = true;
                        break;
                    case NodeState.SUCCESS:
                        break;
                    case NodeState.FAILURE:
                        nodeState = NodeState.FAILURE;
                        return nodeState;
                    default:
                        break;
                }
            }

            nodeState = isAnyNodeRunning ? NodeState.RUNNING : NodeState.SUCCESS;

            return nodeState;
        }

        public void AddNode(Node _n)
        {
            nodes.Add(_n);
        }
    }
}