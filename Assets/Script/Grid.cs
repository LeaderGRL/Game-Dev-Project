using CodeMonkey.Utils;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Grid
{
    private int width;
    private int height;
    private float cellCize;
    private Vector3 originPosition;
    private int[,] gridArray;
    private TextMesh[,] debugTextArray;

    public Grid(int width, int height, float cellCize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellCize = cellCize;
        this.originPosition = originPosition;

        gridArray = new int[width, height];
        debugTextArray = new TextMesh[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = 1;
                debugTextArray[x, y] = UtilsClass.CreateWorldText(
                    gridArray[x, y].ToString(), 
                    null, 
                    GetWorldPosition(x, y) + new Vector3(cellCize, 0, cellCize) / 2,
                    100,
                    Color.cyan,
                    TextAnchor.MiddleCenter);


                debugTextArray[x, y].transform.Rotate(new Vector3(90, 0, 0));
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }

    private Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0.01f, z) * cellCize + originPosition;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int z)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellCize);
        z = Mathf.FloorToInt((worldPosition - originPosition).z / cellCize);
    }

    public void SetValue(int x, int y, int value)
    {
        if (x >= 0 && y>= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            debugTextArray[x, y].text = value.ToString();
        }
    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        int x, z;
        GetXY(worldPosition, out x, out z);
        SetValue(x, z, value);
    }

    public int GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        return 0;
    }

    public int GetValue(Vector3 worldPosition)
    {
        int x, z;
        GetXY(worldPosition, out x, out z);
        return GetValue(x, z);
    }
}
