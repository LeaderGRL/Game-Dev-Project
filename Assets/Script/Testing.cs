using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class Testing : MonoBehaviour
{
    [SerializeField]
    private float cameraSpeed = 20f;
    public Grid grid;
    [SerializeField] private Camera mainCamera;
    public int GridLength;
    public int GridWidth;
    public GameObject BuildingToCreate;

    // Start is called before the first frame update
    void Start()
    {
        //grid = new Grid(GridLength, GridWidth, 16f, new Vector3(-25 * GridLength, 0, -25 * GridWidth));
        grid = new Grid(GridLength, GridWidth, 16f, new Vector3(-8 * GridLength, 0, -8 * GridWidth));
    }

    private void Update()
    {
        Vector3 mousePosition = Vector3.zero;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            mousePosition = raycastHit.point;
            if (grid.GetValue(mousePosition) == 0)
            {
                int cellX = 0;
                int cellY = 0;
                grid.GetXY(mousePosition, out cellX, out cellY);
                BuildingToCreate.transform.position = grid.GetWorldPosition(cellX, cellY) + new Vector3(8,0,8);
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (grid.GetValue(mousePosition) == 0)
            {
                int cellX = 0;
                int cellY = 0;
                grid.GetXY(mousePosition, out cellX, out cellY);
                GameObject newBuilding = Instantiate(BuildingToCreate);
                newBuilding.transform.position = grid.GetWorldPosition(cellX, cellY) + new Vector3(8, 0, 8);
            }
            grid.SetValue(mousePosition,  1);
        }
        if (Input.GetMouseButtonDown(1))
        {
            print(grid.GetValue(mousePosition));
        }

        if (Input.GetAxis("Vertical") != 0)
        {
            transform.position += Vector3.up * Time.deltaTime * cameraSpeed * Input.GetAxis("Vertical");
        }
        if (Input.GetAxis("Horizontal") != 0)
        {
            float speed = Time.deltaTime * cameraSpeed * Input.GetAxis("Horizontal");
            transform.position += new Vector3( 1* speed, 0, -1*speed);
        }

    }

}
