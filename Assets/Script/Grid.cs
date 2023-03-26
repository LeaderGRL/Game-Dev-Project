using CodeMonkey.Utils;
using UnityEngine;

public class Grid
{
    private int width;
    private int height;
    private float cellCize;
    private int[,] gridArray;
    private TextMesh[,] debugTextArray;

    public Grid(int width, int height, float cellCize)
    {
        this.width = width;
        this.height = height;
        this.cellCize = cellCize;

        gridArray = new int[width, height];
        debugTextArray = new TextMesh[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                debugTextArray[x, y] = UtilsClass.CreateWorldText(
                    gridArray[x, y].ToString(), 
                    null, 
                    GetWorldPosition(x, y) + new Vector3(cellCize, 0, cellCize) / 2,
                    100,
                    Color.cyan,
                    TextAnchor.MiddleCenter);


                debugTextArray[x, y].transform.Rotate(new Vector3(90, 0, 0));
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.red, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.red, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.red, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.red, 100f);

        SetValue(2, 1, 56);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, 0.01f, y) * cellCize;
    }

    public void SetValue(int x, int y, int value)
    {
        if (x >= 0 && y>= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            debugTextArray[x, y].text = value.ToString();
        }
    }
}
