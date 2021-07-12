using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsButtonController : MonoBehaviour
{
    public GameObject panel;
    public GameObject optionsButton;
    CameraController cameraController;
    GameObject[] gameObjects; 
    Text text;
    void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        text = panel.GetComponentInChildren<Text>();
        gameObjects = GameObject.FindGameObjectsWithTag("Sprite");
    }

    public void Click()
    {
        panel.SetActive(true);
        cameraController.muveEnabled = false;
        optionsButton.SetActive(false);

        Vector3 upperLeftCorner = cameraController.CalcPosition(new Vector3(0, Screen.height, 0));
        float minDistance = Vector3.Distance(gameObjects[0].transform.position, upperLeftCorner);
        text.text = gameObjects[0].name;
        for (int i=0; i < gameObjects.Length; i++)
        {
            float distance = Vector2.Distance(gameObjects[i].transform.position, upperLeftCorner);
            if (distance<minDistance)
            {
                minDistance = distance;
                text.text = gameObjects[i].name;
            }
        }
    }
}
