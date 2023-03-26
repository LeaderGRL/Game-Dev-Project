using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform seeker, target;
    
    private Grid grid;
    private PathRequestManager requestManager;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    public void FindPath(PathRequest request, Action<PathResult> callback)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        // Initialize variables
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        // Get start and end nodes from the grid
        Node startNode = grid.NodeFromWorldPoint(request.pathStart);
        Node targetNode = grid.NodeFromWorldPoint(request.pathEnd);

        // Check if both start and end nodes are walkable
        if (startNode.walkable && targetNode.walkable)
        {
            // OpenSet => Node to be evalueted
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize); 
            HashSet<Node> closedSet = new HashSet<Node>();

            // Add start node to open set
            openSet.Add(startNode);

            // Loop until the open set is empty
            while (openSet.Count > 0)
            {
                // Get the node with the lowest F cost from the open set
                Node currentNode = openSet.RemoveFirst();

                // Add current node to the closed set
                closedSet.Add(currentNode);

                // If we have reached the target node, stop the search
                if (currentNode == targetNode)
                {
                    // Stop the timer and print the elapsed time
                    sw.Stop();
                    print("Path found : " + sw.ElapsedMilliseconds + "ms");
                    pathSuccess = true;
                    break;
                }

                // Loop through the neighbours of the current node
                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    // Skip this neighbour if it's not walkable or already in the closed set
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    // Calculate the new cost to move to this neighbour
                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) + neighbour.movementPenalty;
                    //UnityEngine.Debug.Log("current.gCost : " + currentNode.gCost + "GetDistance(currentNode, neighbour) :  " + GetDistance(currentNode, neighbour));

                    // If the new cost is lower than the neighbour's current cost or it's not in the open set, update its values and add it to the open set
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                        else
                        {
                            openSet.UpdateItem(neighbour);
                        }
                    }
                }
            }
        }

        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
            pathSuccess = waypoints.Length > 0;
        }

        callback(new PathResult(waypoints, pathSuccess, request.callback));
    }

    // This method retraces a path from the end node to the start node
    // by following the parent nodes of each node in the path.
    // It then simplifies the path by removing unnecessary waypoints.
    // The resulting waypoints are returned as a Vector3 array.

    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        // Create an empty list to store the path
        List<Node> path = new List<Node>();

        // Start at the end node
        Node currentNode = endNode;

        // Traverse the path by following the parent nodes of each node
        while (currentNode != startNode)
        {
            // Add the current node to the path list
            path.Add(currentNode);

            // Move to the parent node
            currentNode = currentNode.parent;
        }

        // Simplify the path by removing unnecessary waypoints
        Vector3[] waypoints = SimplifyPath(path);

        // Reverse the order of the waypoints so they start from the start node
        Array.Reverse(waypoints);

        // Return the simplified waypoints as a Vector3 array
        return waypoints;
    }

    // This method simplifies a path by removing waypoints that do not change direction.
    // It returns the simplified waypoints as a list of Vector3 points.

    Vector3[] SimplifyPath(List<Node> path)
    {
        // Create an empty list to store the waypoints
        List<Vector3> waypoints = new List<Vector3>();

        // Initialize the old direction to zero
        Vector2 directionOld = Vector2.zero;

        // If the path contains more than one node, simplify it
        if (path.Count != 1)
        {
            // Loop through all nodes in the path except the last one
            for (int i = 0; i < path.Count - 1; i++)
            {
                // Calculate the direction from the current node to the next node
                Vector2 directionNew = new Vector2(path[i].gridX - path[i + 1].gridX, path[i].gridY - path[i + 1].gridY);

                // If the direction changes, add the current node's position to the list of waypoints
                if (directionNew != directionOld)
                {
                    waypoints.Add(path[i].worldPosition);
                }

                // Update the old direction to the new direction
                directionOld = directionNew;
            }
        }
        else // If the path only contains one node, add it to the list of waypoints
        {
            waypoints.Add(path[0].worldPosition);
        }

        // Return the simplified waypoints as a Vector3 array
        return waypoints.ToArray();
    }

    // This method calculates the distance between two nodes using the Manhattan distance formula
    // It returns the distance as an integer.
    int GetDistance(Node nodeA, Node nodeB)
    {
        // Calculate the horizontal distance between the nodes
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);

        // Calculate the vertical distance between the nodes
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        // Calculate the diagonal distance (if any) using the diagonal movement cost
        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);
        else
            return 14 * distX + 10 * (distY - distX);
    }
}
