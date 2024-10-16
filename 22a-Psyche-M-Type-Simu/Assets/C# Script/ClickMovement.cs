using UnityEngine;

public class ClickMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody rb;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private float holdDuration = 0.3f; // Time to differentiate click vs hold
    private float pressTime; // Time the button is pressed
    private bool isPressing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Handle mouse input
        if (Input.GetMouseButtonDown(0)) // Left mouse button press
        {
            pressTime = Time.time; // Record the time when pressed
            isPressing = true; // Start pressing
        }

        if (Input.GetMouseButton(0) && isPressing) // While the button is held down
        {
            // Check if hold duration exceeded
            if (Time.time - pressTime > holdDuration)
            {
                isPressing = false; // Stop pressing
            }
        }

        if (Input.GetMouseButtonUp(0) && isPressing) // On mouse button release
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Perform raycast
            if (Physics.Raycast(ray, out hit))
            {
                targetPosition = hit.point; // Set target position to the clicked point
                isMoving = true; // Start moving the ball
            }

            isPressing = false; // Reset pressing state
        }

        // Touch input handling
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                pressTime = Time.time; // Record the time when pressed
                isPressing = true; // Start pressing
            }

            if (touch.phase == TouchPhase.Moved && isPressing)
            {
                // If it moves and still holding, we do nothing (it's a drag).
                return;
            }

            if (touch.phase == TouchPhase.Ended && isPressing)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                // Perform raycast
                if (Physics.Raycast(ray, out hit))
                {
                    targetPosition = hit.point; // Set target position to the touched point
                    isMoving = true; // Start moving the ball
                }

                isPressing = false; // Reset pressing state
            }
        }

        // Move towards the target position
        if (isMoving)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            rb.velocity = new Vector3(direction.x * moveSpeed, rb.velocity.y, direction.z * moveSpeed);

            // Stop moving if close enough to the target
            if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
            {
                isMoving = false;
                rb.velocity = Vector3.zero; // Stop the ball completely
            }
        }
    }
}
