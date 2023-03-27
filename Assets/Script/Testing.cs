using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Testing : MonoBehaviour
{
    private Grid grid;
    [SerializeField] private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid(4, 2, 50f, new Vector3(-51, 0, 0));
    }

    private void Update()
    {
        Vector3 mousePosition = Vector3.zero;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            mousePosition = raycastHit.point;
        }
        if (Input.GetMouseButtonDown(0))
        {
            grid.SetValue(mousePosition, 56);
        }
        if (Input.GetMouseButtonDown(1))
        {
            print(grid.GetValue(mousePosition));
        }
    }

}
