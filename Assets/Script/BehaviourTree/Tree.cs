using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class Tree : MonoBehaviour
    {
        // Define a private reference to the root node of the behavior tree
        private Node _root = null;
        
        // Define an abstract SetupTree method that must be implemented by child classes to set up the behavior tree
        protected abstract Node SetupTree();

        // When the object is started, call the SetupTree method to initialize the behavior tree
        protected void Start()
        {
            _root = SetupTree();
        }

        // Update the behavior tree each frame by calling the Evaluate method on the root node
        private void Update()
        {
            if (_root != null)
            {
                _root.Evaluate();
            }
        }
    }
}
