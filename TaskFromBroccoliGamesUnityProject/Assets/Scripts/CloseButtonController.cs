using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseButtonController : MonoBehaviour
{
    public GameObject panel;
    public GameObject optionsButton;
    CameraController cameraController;

    void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }

  
    void Update()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    public void Click ()
    {
        panel.SetActive(false);
        cameraController.muveEnabled = true;
        optionsButton.SetActive(true);
    }
}
