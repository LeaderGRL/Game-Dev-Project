/************************************************************************************************************************
    This class defines a single node in a grid-based pathfinding system.
    It contains information about whether the node is walkable or not, 
    its position in the world, its position in the grid, and any movement penalties associated with it. 
    It also tracks the cost to reach this node (gCost),
    the estimated cost to reach the target node (hCost), and the total cost(fCost = gCost + hCost). 
    Finally, it tracks the node's parent in the path.
************************************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    // State of the node (can it be walked on or not)
    public bool walkable;

    // Position of the node in the world
    public Vector3 worldPosition;

    // Position of the node in the grid
    public int gridX;
    public int gridY;

    // Movement penalty for traversing this node
    public int movementPenalty;

    // Cost to reach this node from the start node
    public int gCost;

    // Estimated cost to reach the target node from this node
    public int hCost;

    // Parent node in the path
    public Node parent;

    // Heap index for use in the priority queue
    private int heapIndex;

    // Constructor for the node class
    public Node(bool _walkable, Vector3 _worldPosition, int _gridX, int _gridY, int _penalty)
    {
        walkable = _walkable;
        worldPosition = _worldPosition;
        gridX = _gridX;
        gridY = _gridY;
        movementPenalty = _penalty;
    }

    // Property to calculate the total cost to reach the node
    public int fCost
    {
        get { return gCost + hCost; }
    }

    // Property for getting and setting the heap index
    public int HeapIndex
    {
        get { return heapIndex; }
        set { heapIndex = value; }
    }

    // Compare nodes based on their total cost (fCost), breaking ties with hCost
    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare; // Return negated compare for use in min-heap
    }
}
