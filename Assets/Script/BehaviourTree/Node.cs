using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    // Define the possible states for a node
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE,
    }

    // Define the base class for a behavior tree node
    public class Node
    {
        // A reference to the parent node
        public Node parent;

        // The current state of the node
        protected NodeState state;

        // A list of child nodes
        protected List<Node> children = new List<Node>();

        // A dictionary to store node-specific data
        private Dictionary<string, object> data = new Dictionary<string, object>();

        // Default constructor
        public Node()
        {
            parent = null;
        }

        // Constructor that takes a list of child nodes
        public Node(List<Node> children)
        {
            // Attach each child node to this parent node
            foreach (Node child in children)
            {
                _Attach(child);
            }
        }

        // Attach a child node to this parent node
        private void _Attach(Node node)
        {
            node.parent = this;
            children.Add(node);
        }

        // The virtual Evaluate method, which can be overridden by child classes
        // to define their specific behavior
        public virtual NodeState Evaluate => NodeState.FAILURE;

        // Set a key-value pair in the node's data dictionary
        public void SetData(string key, object value)
        {
            data[key] = value;
        }

        // Get the value associated with a key in the node's data dictionary
        public object GetData(string key)
        {
            object value = null;

            // Check if the key exists in this node's data dictionary
            if (data.TryGetValue(key, out value))
            {
                return value;
            }

            // If the key doesn't exist in this node's data dictionary, check the parent node
            Node node = parent;

            while (node != null)
            {
                value = node.GetData(key);

                // If the value is found in a parent node, return it
                if (value != null)
                {
                    return value;
                }

                node = node.parent;
            }

            // If the key isn't found in any node, return null
            return null;
        }

        // Define a method to remove a key-value pair from the node's data dictionary
        public bool ClearData(string key)
        {
            if (data.ContainsKey(key))
            {
                data.Remove(key);
                return true;
            }

            // If the value isn't in this node's data dictionary, look in the parent node's data dictionary
            Node node = parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                    return true;

                node = node.parent;
            }

            // If the key-value pair isn't found in any data dictionary, return false
            return false;
        }
    }
}