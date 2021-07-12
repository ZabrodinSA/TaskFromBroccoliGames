using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViewsComponent;

public class CameraController : MonoBehaviour
{
    public float cameraGain = 1.0f;
    public float zoomGain = 0.5f;
    public GameObject stage;
    public bool muveEnabled = true;
    private float cameraSpeed = 1.0f;
    private float mapEdgeRight;
    private float mapEdgeLeft;
    private float mapEdgeTop;
    private float mapEdgeBot;

    static Plane plane;
    void Start()
    {
        cameraSpeed *= cameraGain;
        plane = new Plane(transform.forward, Vector3.zero);

        DataController dataController = stage.GetComponent<DataController>();
        mapEdgeRight = dataController.mapEdgeRight;
        mapEdgeLeft = dataController.mapEdgeLeft;
        mapEdgeTop = dataController.mapEdgeTop;
        mapEdgeBot = dataController.mapEdgeBot;
        
        Vector3 startPosition = CalcPosition(new Vector3(Screen.width, 0, 0));
        transform.position = new Vector3 (startPosition.x + mapEdgeLeft, startPosition.y + mapEdgeTop, transform.position.z);
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && muveEnabled)
        {
            CameraMovement();
        }
        if (Input.mouseScrollDelta.y != 0 && muveEnabled)
        {
            CameraZoom();
        }
    }

    private void CameraMovement ()
    {
        float deltaX = Input.GetAxis("Mouse X") * cameraSpeed;
        float deltaY = Input.GetAxis("Mouse Y") * cameraSpeed;
        Vector3 cameraEdgeRightTop = CalcPosition(new Vector3(Screen.width, Screen.height, 0));
        Vector3 cameraEdgeLeftBot = CalcPosition(new Vector3(0, 0, 0));
        if (deltaX < 0 && cameraEdgeRightTop.x < mapEdgeRight)
        {
            transform.position = new Vector3(transform.position.x - deltaX, transform.position.y, transform.position.z);
        }
        if (deltaX > 0 && cameraEdgeLeftBot.x > mapEdgeLeft)
        {

            transform.position = new Vector3(transform.position.x - deltaX, transform.position.y, transform.position.z);
        }
        if (deltaY < 0 && cameraEdgeRightTop.y < mapEdgeTop)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - deltaY, transform.position.z);
        }
        if (deltaY > 0 && cameraEdgeLeftBot.y > mapEdgeBot)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - deltaY, transform.position.z);
        }



    }

    private void CameraZoom()
    {
        Camera.main.orthographicSize = Camera.main.orthographicSize - Input.mouseScrollDelta.y*zoomGain;
        if (Camera.main.orthographicSize < 0.01)
        {
            Camera.main.orthographicSize = 0.01f;
        }
        cameraSpeed = cameraGain * Camera.main.orthographicSize / 5;
    }


    public Vector3 CalcPosition(Vector3 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        float dist = 0f;
        Vector3 pos = Vector3.zero;

        if (plane.Raycast(ray, out dist))
        {
            pos = ray.GetPoint(dist);
        }
        return pos;
    }
}
