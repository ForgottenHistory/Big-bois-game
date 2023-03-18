using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 0.5f;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;   
    }

    void Update()
    {
        MouseLook();
    }

    void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 newRotation = transform.localEulerAngles;
        newRotation.x -= mouseY * mouseSensitivity;
        newRotation.y += mouseX * mouseSensitivity;
        transform.localEulerAngles = newRotation;

        transform.parent.Rotate(Vector3.up * mouseX * mouseSensitivity);
    }
}
