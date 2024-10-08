using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;  // The ball (player) to rotate around
    public float distance = 10f;  // Distance from the player
    public float rotationSpeed = 100f;  // Speed of camera rotation
    public float verticalOffset = 2f;  // Height offset to position the camera above the target

    private float currentX = 0f;
    private float currentY = 0f;

    void Update()
    {
        // Check if the left mouse button is held down or if there is a touch
        if (Input.GetMouseButton(0)) // Left mouse button is held
        {
            // Update rotation based on mouse/touch movement
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            currentX += mouseX * rotationSpeed * Time.deltaTime;
            currentY -= mouseY * rotationSpeed * Time.deltaTime;
        }
    }

    void LateUpdate()
    {
        // Update camera position and rotation around the target (ball)
        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        // Set camera position, adding the vertical offset after rotation
        transform.position = target.position + rotation * direction + new Vector3(0, verticalOffset, 0);

        // Make the camera look at the target
        transform.LookAt(target.position);  // Camera will now focus directly on the target
    }
}
