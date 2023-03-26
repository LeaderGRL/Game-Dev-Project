using CodeMonkey.Utils;
using UnityEngine;

public class Grid
{
    private int width;
    private int height;
    private float cellCize;
    private int[,] gridArray;

    public Grid(int width, int height, float cellCize)
    {
        this.width = width;
        this.height = height;
        this.cellCize = cellCize;

        gridArray = new int[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                TextMesh newTewtMesh = UtilsClass.CreateWorldText(
                    gridArray[x, y].ToString(), 
                    null, 
                    GetWorldPosition(x, y),
                    100,
                    Color.white,
                    TextAnchor.MiddleCenter);

                
                newTewtMesh.transform.Rotate(new Vector3(-90, 0, 0));
            }
        }
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, 0, y) * cellCize;
    }
}
