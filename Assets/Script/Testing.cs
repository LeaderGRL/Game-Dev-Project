using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Testing : MonoBehaviour
{
    [SerializeField]
    private float cameraSpeed = 20f;
    private Grid grid;
    [SerializeField] private Camera mainCamera;
    public int GridLength;
    public int GridWidth;

    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid(GridLength, GridWidth, 50f, new Vector3(-25 * GridLength, 0, -25 * GridWidth));
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
