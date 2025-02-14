using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDragCamera : MonoBehaviour
{
    public float rotationSpeed = 5f;
    private Vector3 lastMousePosition;
    private bool isDragging = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            float yaw = delta.x * rotationSpeed * Time.deltaTime;
            float pitch = -delta.y * rotationSpeed * Time.deltaTime;

            transform.Rotate(Vector3.up, yaw, Space.World);
            transform.Rotate(Vector3.right, pitch, Space.Self);

            lastMousePosition = Input.mousePosition;
        }
    }
}
