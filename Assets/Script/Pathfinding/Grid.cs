/*
 * This script creates a grid of nodes in the game world for pathfinding purposes. 
 * It uses a raycast to detect walkable terrain and calculates movement penalties based on terrain types. 
 * It also implements a penalty blur to help smooth the pathfinding calculations. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public bool displayGridGizmos;
    public Transform player;
    public LayerMask unwalkableLayerMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public TerrainTypes[] walkableRegions;
    public int blurWeightSize = 3;
    public int obstacleProximityPenalty = 10;
    //public List<Node> path;

    private Node[,] grid;
    private float nodeDiameter;
    private int gridSizeX, gridSizeY;
    private LayerMask walkableMask;
    private Dictionary<int, int> walkableRegionDictionary = new Dictionary<int, int>();
    private int penaltyMin = int.MaxValue;
    private int penaltyMax = int.MinValue;

    private void Awake()
    {
        // Calculate the diameter and size of each node in the grid
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        // Combine the layer masks of all the walkable terrain types
        foreach (TerrainTypes region in walkableRegions)
        {
            walkableMask.value |= region.terrainMask.value;

            // Add the movement penalty for the terrain type to the dictionary
            walkableRegionDictionary.Add((int)Mathf.Log(region.terrainMask.value, 2), region.terrainPenalty);
        }

        // Create the grid of nodes
        CreateGrid();
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                // Calculate the world position of the current node.
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);

                // Check if the current node is walkable by using a sphere check with the unwalkable layer mask.
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableLayerMask));

                int movementPenalty = 0;

                // Perform a raycast downwards from the current node and check the hit collider's layer against the walkable region dictionary to determine the movement penalty.
                Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100, walkableMask))
                {
                    walkableRegionDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                }

                // Apply an additional obstacle proximity penalty if the current node is not walkable.
                if (!walkable)
                {
                    movementPenalty += obstacleProximityPenalty;
                }

                // Create a new node with the calculated properties and store it in the grid array.
                grid[x, y] = new Node(walkable, worldPoint, x, y, movementPenalty);
            }
        }

        // Apply a blur penalty map to the grid to smooth out the movement penalties.
        BlurPenaltyMap(blurWeightSize);
    }

    void BlurPenaltyMap(int blurSize)
    {
        int kernelSize = blurSize * 2 + 1;
        int kernelExtents = (kernelSize - 1) / 2;

        int[,] penaltiesHorizontalPass = new int[gridSizeX, gridSizeY];
        int[,] penaltiesVerticalPass = new int[gridSizeX, gridSizeY];

        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = -kernelExtents; x <= kernelExtents; x++)
            {
                int sampleX = Mathf.Clamp(x, 0, kernelExtents);
                penaltiesHorizontalPass[0, y] += grid[sampleX, y].movementPenalty;
            }

            for (int x = 1; x < gridSizeX; x++)
            {
                int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, gridSizeX);
                int addIndex = Mathf.Clamp(x + kernelExtents, 0, gridSizeX - 1);

                penaltiesHorizontalPass[x, y] = penaltiesHorizontalPass[x - 1, y] - grid[removeIndex, y].movementPenalty + grid[addIndex, y].movementPenalty;
            }
        }

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = -kernelExtents; y <= kernelExtents; y++)
            {
                int sampleY = Mathf.Clamp(y, 0, kernelExtents);
                penaltiesVerticalPass[x, 0] += penaltiesHorizontalPass[x, sampleY];
            }

            int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, 0] / (kernelSize * kernelExtents));
            grid[x, 0].movementPenalty = blurredPenalty;

            for (int y = 1; y < gridSizeY; y++)
            {
                int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, gridSizeY);
                int addIndex = Mathf.Clamp(y + kernelExtents, 0, gridSizeY - 1);

                penaltiesVerticalPass[x, y] = penaltiesVerticalPass[x, y - 1] - penaltiesHorizontalPass[x, removeIndex] + penaltiesHorizontalPass[x, addIndex];
                blurredPenalty =  Mathf.RoundToInt((float)penaltiesVerticalPass[x, y] / (kernelSize * kernelExtents));
                grid[x, y].movementPenalty = blurredPenalty;

                if (blurredPenalty > penaltyMax)
                {
                    penaltyMax = blurredPenalty;
                }

                if (blurredPenalty < penaltyMin)
                {
                    penaltyMin = blurredPenalty;
                }
            }
        }

    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if (grid != null && displayGridGizmos)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(penaltyMin, penaltyMax, n.movementPenalty));

                Gizmos.color = (n.walkable) ? Gizmos.color : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter));
            }
        }

        //Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        //if (grid != null)
        //{
        //    Node playerNode = NodeFromWorldPoint(player.transform.position);

        //    if (onlyDisplayPathGizmos)
        //    {
        //        if (path != null)
        //        {
        //            foreach (Node n in path)
        //            {
        //                Gizmos.color = Color.black;
        //                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
        //            }
        //        }
        //    }
        //    else
        //    {
        //        foreach (Node n in grid)
        //        {
        //            Gizmos.color = n.walkable ? Color.white : Color.red;
        //            if (playerNode == n)
        //            {
        //                Gizmos.color = Color.cyan;
        //            }

        //            if (path != null)
        //            {
        //                if (path.Contains(n))
        //                    Gizmos.color = Color.black;
        //            }
        //            Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
        //        }
        //    }
        //}
    }
}
