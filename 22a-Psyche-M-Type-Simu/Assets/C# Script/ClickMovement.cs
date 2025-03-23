using UnityEngine;
using System.Collections; // Required for IEnumerator

public class ClickMovement : MonoBehaviour
{
    public float rotationSpeed = 5f; // rotation speed when moving
    public float moveSpeed = 5f; // robot speed
    public Vector3 targetPosition; // robot position to go to
    private bool isMoving = false; 
    private float distanceThreshold = 1f; // threshold to consider movement as complete
    private Rigidbody rb;

    // Added audio source and variables for audio control
    public AudioSource movementAudio; // assign the audio source via the inspector
    private float originalVolume;
    private bool isFadingOut = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (movementAudio != null)
        {
            originalVolume = movementAudio.volume;
        }
    }
    
    void Update()
    {
        // when mouse input is right-click
        if (Input.GetMouseButtonDown(1))
        {
            SetTargetPosition();
        }

        // if the object is moving then rotate towards target
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
            isMoving = true;
        }
    }

    void RotateTowardsTarget()
    {
        // calculates the direction to the target
        Vector3 directionToTarget = targetPosition - transform.position;
        directionToTarget.y = 0; // keeps rotation on the x and z axis exclusively

        if (directionToTarget.magnitude > distanceThreshold)
        {
            // smoothly rotate towards the target with Quaternion.Slerp
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            MoveTowardsTarget();
        }
        else
        {
            // Once close enough, mark movement as complete so the audio can fade out.
            isMoving = false;
        }
    }

    void MoveTowardsTarget()
    {
        // Move towards the target position
        if (Vector3.Distance(transform.position, targetPosition) > distanceThreshold)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            // once destination is reached, stop
            isMoving = false;
        }
    }

    // Added LateUpdate to handle audio state without modifying existing Update logic
    void LateUpdate()
    {
        HandleMovementAudio();
    }

    // Function to handle playing and fading out the movement audio
    void HandleMovementAudio()
    {
        if (movementAudio == null)
            return;

        if (isMoving)
        {
            // If moving and audio is not playing, start playing
            if (!movementAudio.isPlaying)
            {
                if (isFadingOut)
                {
                    StopCoroutine("FadeOutAudio");
                    isFadingOut = false;
                    movementAudio.volume = originalVolume;
                }
                movementAudio.Play();
            }
        }
        else
        {
            // If not moving and audio is playing, start fading out
            if (movementAudio.isPlaying && !isFadingOut)
            {
                StartCoroutine(FadeOutAudio(1f)); // Fade out over 1 second
            }
        }
    }

    // Coroutine to gradually fade out the audio
    IEnumerator FadeOutAudio(float fadeTime)
    {
        isFadingOut = true;
        float startVolume = movementAudio.volume;

        while (movementAudio.volume > 0)
        {
            movementAudio.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        movementAudio.Stop();
        movementAudio.volume = originalVolume;
        isFadingOut = false;
    }
}
