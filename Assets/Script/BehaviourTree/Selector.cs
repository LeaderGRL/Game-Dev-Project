using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    // Define a Selector class that inherits from the Node class
    public class Selector : Node
    {
        // Define constructors for the Selector class that call the base constructor
        public Selector() : base() { }
        public Selector(List<Node> children) : base(children) { }

        // Override the Evaluate method to implement the behavior of the Selector node
        public override NodeState Evaluate()
        {
            // Iterate over each child node
            foreach (Node node in children)
            {
                // Evaluate the child node and switch on its return value
                switch (node.Evaluate())
                {
                    // If the child node returns FAILURE, the selector continue to search for a success/running node
                    case NodeState.FAILURE:
                        continue;
                    // If the child node returns SUCCESS, continue to the next child
                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        return state;
                    // If the child node returns RUNNING, continue to the next child
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return state;
                    default:
                        continue;
                }
            }

            // If all child nodes return FAILURE, the selector fail
            state = NodeState.FAILURE;
            return state;
        }
    }
}
