using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using BehaviorTree;

namespace BehaviorTree
{
    [System.Serializable]
    public class Selector : Node
    {
        public List<Node> nodes = new List<Node>();

        public override NodeState Evaluate(BlackBoard _bb)
        {

            foreach (Node n in nodes)
            {
                switch (n.Evaluate(_bb))
                {
                    case NodeState.RUNNING:
                        nodeState = NodeState.RUNNING;
                        return nodeState;
                    case NodeState.SUCCESS:
                        nodeState = NodeState.SUCCESS;
                        return nodeState;
                    case NodeState.FAILURE:
                        break;
                    default:
                        break;
                }
            }

            nodeState = NodeState.FAILURE;
            return NodeState;
        }

        public void AddNode(Node _n)
        {
            nodes.Add(_n);
        }
    }
}