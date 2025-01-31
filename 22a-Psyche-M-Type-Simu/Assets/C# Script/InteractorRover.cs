using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class InteractorRover : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    [SerializeField] private Transform interactionP;
    [SerializeField] private float interactionR = 0.5f;
    [SerializeField] private LayerMask interactionM;
    [SerializeField] GameObject LevelCompleteScreen;
    [SerializeField] GameObject Status0;
    [SerializeField] GameObject Status1;
    [SerializeField] GameObject Status2;
    [SerializeField] Camera drillCamera; // Reference to the drill camera
    [SerializeField] RawImage drillCameraUI; // Reference to the UI panel
    [SerializeField] GameObject Status3;
    [SerializeField] GameObject StatusComplete;
    [SerializeField] GameObject ControlPrompt;

    private readonly Collider[] RovColliders = new Collider[3];
    [SerializeField] private int numInteractFound;
    [SerializeField] private MonoBehaviour movementScript;

    //counts the number of interactions the rover has had
    private int interactionCount = 0;
    private void Start()
    {
        if (drillCamera != null && drillCameraUI != null)
        {
            drillCamera.enabled = false; // Disable the drill camera initially
            drillCameraUI.gameObject.SetActive(false); // Hide the UI panel
        }
        animator = GetComponent<Animator>();
        Status0.SetActive(true);

        StartCoroutine(DisableMovementForSeconds(3));
    }

    private void ShowDrillCamera()
    {
        if (drillCamera != null && drillCameraUI != null)
        {
            drillCamera.enabled = true; // Enable the drill camera
            drillCameraUI.gameObject.SetActive(true); // Show the UI panel
        }
    }

    private void HideDrillCamera()
    {
        if (drillCamera != null && drillCameraUI != null)
        {
            drillCamera.enabled = false; // Disable the drill camera
            drillCameraUI.gameObject.SetActive(false); // Hide the UI panel
        }
    }

    private void Update()
    {
        numInteractFound = Physics.OverlapSphereNonAlloc(interactionP.position, interactionR, RovColliders, interactionM);

        if(numInteractFound > 0)
        {
            var interactable = RovColliders[0].GetComponent<IInteractable>();

            ControlPrompt.SetActive(true);

            if (interactable != null && Input.GetKey(KeyCode.E))
            {
                ControlPrompt.SetActive(false);
                interactable.Interact(this);
                animator.SetTrigger("ActivateDrill");

                // Show the drill camera view
                ShowDrillCamera();

                RovColliders[0].gameObject.layer = LayerMask.NameToLayer("Uninteractable");

                interactionCount++;
                missionStatus(interactionCount);
                Debug.Log(interactionCount);

                StartCoroutine(DisableMovementForSeconds(6));

                if (interactionCount >= 4)
                {
                    StartCoroutine(LevelCompleteRoutine());
                }
            }
        }
    }

    private IEnumerator DisableMovementForSeconds(float seconds)
    {
        if (movementScript != null)
        {
            movementScript.enabled = false;

            yield return new WaitForSeconds(seconds);

            movementScript.enabled = true;

            // Hide the drill camera view after movement resumes
            HideDrillCamera();
        }
    }

    private IEnumerator LevelCompleteRoutine()
    {
        Debug.Log("Level complete"); 
        yield return new WaitForSeconds(3); //lets animation play for 3 secnds
        LevelCompleteScreen.SetActive(true); 
        Time.timeScale = 0f; //pauses further action
    }

    private void missionStatus(int interactionCount)
    {
        //mission progress text, changes as the mission progresses\
        switch (interactionCount)
        {
            case 1:
                Status0.SetActive(false);
                Status1.SetActive(true);
                break;
            case 2:
                Status1.SetActive(false);
                Status2.SetActive(true);
                break;
            case 3:
                Status2.SetActive(false);
                Status3.SetActive(true);
                break;
            default:
                Status3.SetActive(false);
                StatusComplete.SetActive(true);
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactionP.position, interactionR);
    }
}
