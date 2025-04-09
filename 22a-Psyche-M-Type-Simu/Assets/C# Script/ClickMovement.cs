using UnityEngine;
using System.Collections;

public class ClickMovement : MonoBehaviour
{
    
    //beofre we used physics variable to push the robot, but would mess up the physics and flip the rover over constantly,
    //now we will not use this so we can have better player physics when on the surface of the asteroid and to not hurt the user experience
    public float rotationSpeed = 5f; //rotation speed when moving
    public float moveSpeed = 5f; //robot speed
    public Vector3 targetPosition; //robot position to go to
    private bool isMoving = false; 
    private float distanceThreshold = 1f; //threshold to consider movement as complete
    private Rigidbody rb;

    //Added for movement sound
    [SerializeField] private AudioSource movementAudioSource;
    [SerializeField] private AudioClip movementClip;
    private Coroutine fadeOutCoroutine; //Track fade coroutine
    private float movementMaxVolume = 0.09f;
    void Start()
{
    rb = GetComponent<Rigidbody>();

    //Ensure only one instance exists
    if (movementAudioSource != null)
    {
        rb = GetComponent<Rigidbody>();
        if (movementAudioSource.isPlaying && movementAudioSource.gameObject != this.gameObject)
        {
            Destroy(movementAudioSource.gameObject);
        }

        movementAudioSource.playOnAwake = false;
        movementAudioSource.loop = true;
        movementAudioSource.clip = movementClip;
        movementAudioSource.volume = movementMaxVolume;
    }
}

    void Update()
    {
        //when mouse input is right-click
        if (Input.GetMouseButtonDown(1))
        {
            SetTargetPosition();
        }

        //if the object is moving then rotate towards target
        if (isMoving)
        {
            RotateTowardsTarget();

            //Play movement sound while moving
            if (movementAudioSource != null && !movementAudioSource.isPlaying)
            {
                if (fadeOutCoroutine != null)
                {
                    StopCoroutine(fadeOutCoroutine); //Cancel fade if resuming movement
                    fadeOutCoroutine = null;
                }
                movementAudioSource.volume = movementMaxVolume; //Reset to capped volume
                movementAudioSource.Play();
            }
        }
        else
        {
            //Fade out movement sound when not moving
            if (movementAudioSource != null && movementAudioSource.isPlaying && fadeOutCoroutine == null)
            {
                fadeOutCoroutine = StartCoroutine(FadeOutAudio(movementAudioSource, 1f));
            }
        }

        //Failsafe: stop sound if destination is reached but isMoving wasn't cleared
        if (isMoving && Vector3.Distance(transform.position, targetPosition) <= distanceThreshold)
        {
            isMoving = false;

            if (movementAudioSource != null && movementAudioSource.isPlaying && fadeOutCoroutine == null)
            {
                fadeOutCoroutine = StartCoroutine(FadeOutAudio(movementAudioSource, 1f));
            }
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
        //calculates the direction to the target
        Vector3 directionToTarget = targetPosition - transform.position;
        directionToTarget.y = 0; //keeps rotation on the x and z axis exclusively

        if (directionToTarget.magnitude > distanceThreshold)
        {
            //smoothly rotate towards the target with Quaternion.Slerp
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
            //once destination is reached, stop
            isMoving = false;
        }
    }

    //Coroutine for fading out audio
    private IEnumerator FadeOutAudio(AudioSource audioSource, float duration)
    {
        float startVolume = movementMaxVolume; //Use max volume as starting point

        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = movementMaxVolume; //Reset to max for next use
        fadeOutCoroutine = null;
    }
}