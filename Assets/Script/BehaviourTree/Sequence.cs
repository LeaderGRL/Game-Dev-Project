using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    // Define a Sequence class that inherits from the Node class
    public class Sequence : Node
    {
        // Define constructors for the Sequence class that call the base constructor
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }

        // Override the Evaluate method to implement the behavior of the Sequence node
        public override NodeState Evaluate()
        {
            // Initialize a variable to keep track of whether any child nodes are currently running
            bool anyChildIsRunning = false;

            // Iterate over each child node
            foreach (Node node in children)
            {
                // Evaluate the child node and switch on its return value
                switch (node.Evaluate())
                {
                    // If the child node returns FAILURE, the sequence fails immediately
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        return state;
                    // If the child node returns SUCCESS, continue to the next child
                    case NodeState.SUCCESS:
                        continue;
                    // If the child node returns RUNNING, set anyChildIsRunning to true and return RUNNING
                    case NodeState.RUNNING:
                        anyChildIsRunning = true;
                        state = NodeState.RUNNING;
                        return state;
                }
            }

            // If all child nodes return SUCCESS, the sequence succeeds
            // If any child node is still running, the sequence continues to run
            state = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return state;
        }
    }
}
