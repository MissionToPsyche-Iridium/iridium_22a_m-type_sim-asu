using UnityEngine;

public class ClickMovement : MonoBehaviour
{
    /*
    public float moveSpeed = 4f;
    private Rigidbody rb;
    public Vector3 targetPosition;
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
        if (Input.GetMouseButtonDown(1)) // Left mouse button press
        {
            pressTime = Time.time; // Record the time when pressed
            isPressing = true; // Start pressing
        }

        if (Input.GetMouseButton(1) && isPressing) // While the button is held down
        {
            // Check if hold duration exceeded
            if (Time.time - pressTime > holdDuration)
            {
                isPressing = false; // Stop pressing
            }
        }

        if (Input.GetMouseButtonUp(1) && isPressing) // On mouse button release
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
    */
    //using UnityEngine;
    public float rotationSpeed = 5f; // How fast the object rotates to face the target
    public float moveSpeed = 5f;     // How fast the object moves to the target
    public Vector3 targetPosition;  // The target position to move to
    private bool isMoving = false;   // Whether the object is moving or rotating
    private float distanceThreshold = 1.5f; // Threshold to consider movement as complete
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        // Check for right-click and set the target position
        if (Input.GetMouseButtonDown(1)) // Right-click
        {
            SetTargetPosition();
        }

        // If the object is moving, rotate it smoothly toward the target
        if (isMoving)
        {
            RotateTowardsTarget();
        }
    }

    void SetTargetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            targetPosition = hit.point;
            //targetPosition = new Vector3(hit.point.x, hit.point.y+0.5f, hit.point.z);
            isMoving = true;
        }
    }

    void RotateTowardsTarget()
    {
        // Calculate the direction to the target
        Vector3 directionToTarget = targetPosition - transform.position;
        directionToTarget.y = 0; // Keep rotation on the horizontal plane

        if (directionToTarget.magnitude > distanceThreshold)
        {
            // Smoothly rotate towards the target using Quaternion.Slerp
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            MoveTowardsTarget();
        }
        
    }

    void MoveTowardsTarget()
    {
        // Move towards the target position
        if (Vector3.Distance(transform.position, targetPosition) > distanceThreshold)
        {
            //Vector3 direction = (targetPosition - transform.position).normalized;
            //rb.velocity = new Vector3(direction.x * moveSpeed, rb.velocity.y, direction.z * moveSpeed);
            //rb.velocity = Vector3.forward * moveSpeed;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            // Once the object reaches the target, stop moving
            isMoving = false;
        }
    }
}
