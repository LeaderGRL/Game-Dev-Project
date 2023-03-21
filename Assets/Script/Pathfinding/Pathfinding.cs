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
                    //UnityEngine.Debug.Log("gCost : " + neighbour.gCost + " - hCost : " + neighbour.hCost + " - Distance : " + GetDistance(currentNode, neighbour) + " - Penalty : " + neighbour.movementPenalty);
                    
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


    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);

        return waypoints;

        //grid.path = path;
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        if (path.Count != 1)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Vector2 directionNew = new Vector2(path[i].gridX - path[i + 1].gridX, path[i].gridY - path[i + 1].gridY);
                if (directionNew != directionOld)
                {
                    waypoints.Add(path[i].worldPosition);
                }
                directionOld = directionNew;
            }
        }
        else
        {
            waypoints.Add(path[0].worldPosition);
        }

        return waypoints.ToArray();
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);

        return 14 * distX + 10 * (distY - distX);

    }
}
