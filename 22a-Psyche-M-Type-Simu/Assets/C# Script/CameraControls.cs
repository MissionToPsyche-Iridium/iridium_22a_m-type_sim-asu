using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    // object that we are orbiting (player)
    public Transform trackedObject;

    // bounds of input effects on camera
    public float moveSpeed = 500;
    public float rotationSpeed = 3f;
    public float maxDistance = 10f;

    // prevents the camera from flipping too far. 
    public float minYAngle = 10f;
    public float maxYAngle = 80f;

    // tracks if we're in orbit mode
    public bool isOrbiting = false;

    private Vector3 currentOffset;  // offset from player
    private float currentDistance;  // zoom distance
    private float currentYRotation; // y-axis rotation angle (pitch)
    private float currentXRotation; // x-axis rotation angle (yaw)

    // Start is called before the first frame update
    void Start()
    {
        currentDistance = maxDistance;
        currentXRotation = 0f;
        currentYRotation = 30f;
        currentOffset = new Vector3(0, 5f, -currentDistance);
    }

    void LateUpdate()
    {
        // zoom in/out with scroll wheel
        currentDistance += Input.GetAxis("Mouse ScrollWheel") * moveSpeed * Time.deltaTime;
        currentDistance = Mathf.Clamp(currentDistance, 1f, maxDistance);

        transform.position = Vector3.MoveTowards(transform.position,
            trackedObject.position - trackedObject.forward * currentDistance,
            10 * Time.deltaTime);

        // activate orbit mode on right-click
        if (Input.GetMouseButtonDown(1)) {
            isOrbiting = true; 
        }
        if(Input.GetMouseButtonUp(1)) {
            isOrbiting = false;
        }

        // rotation handles
        if(isOrbiting) {
            // get mouse position
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // adjust orbit position based on mouse movements
            currentXRotation += mouseX * rotationSpeed;
            currentYRotation -= mouseY * rotationSpeed; // inverted

            // clamp vertical rotation to prevent camera flippin gon y axis
            currentYRotation = Mathf.Clamp(currentYRotation, minYAngle, maxYAngle);
        }

        Quaternion rotation = Quaternion.Euler(currentYRotation, currentXRotation, 0);
        currentOffset = new Vector3(0, 0, -currentDistance);
        Vector3 finalPosition = trackedObject.position + rotation * currentOffset;

        transform.position = finalPosition;
        transform.LookAt(trackedObject);

    }
}
