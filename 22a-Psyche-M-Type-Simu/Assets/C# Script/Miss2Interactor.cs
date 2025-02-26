using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class Miss2Interactor : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    [SerializeField] private Transform interactionP;
    [SerializeField] private float interactionR = 0.5f;
    [SerializeField] private LayerMask interactionM;
    [SerializeField] GameObject ControlPrompt;
    private readonly Collider[] RovColliders = new Collider[3];
    [SerializeField] private int numInteractFound;
    [SerializeField] private MonoBehaviour movementScript;

    [SerializeField] public Camera mainCamera;
    [SerializeField] public Camera camMode;
    private bool isCamModeActive = false;

    [SerializeField] private GameObject target1;


    //counts the number of interactions the rover has had
    private int interactionCount = 0;
    private void Start()
    {
        animator = GetComponent<Animator>();
        //Status0.SetActive(true);
        mainCamera.enabled = true;
        camMode.enabled = false;

        //StartCoroutine(DisableMovementForSeconds(3));
    }

    private void Update()
    {
        numInteractFound = Physics.OverlapSphereNonAlloc(interactionP.position, interactionR, RovColliders, interactionM);

        if (numInteractFound > 0)
        {
            var interactable = RovColliders[0].GetComponent<Miss2IInteractable>();

            ////enables display UI for what key to press to interact with surface
            ControlPrompt.SetActive(true);

            if (interactable != null && Input.GetKey(KeyCode.E))
            {
                //disables display UI for what key to press
                ControlPrompt.SetActive(false);

                //selects interactable object
                interactable.Interact(this);

                //starts camera animation
                animator.SetTrigger("CameraActiv");

                mainCamera.enabled = false;
                camMode.enabled = true;
                isCamModeActive = true;

                //disables movement till animation is complete
                movementScript.enabled = false;

                //check if all objectives have been completed
                
            }
            else if (numInteractFound > 0 && Input.GetKey(KeyCode.R) && isCamModeActive == true) {

                bool visibility = visCheck();

                if (visibility)
                {
                    camMode.enabled = false;
                    mainCamera.enabled = true;
                    isCamModeActive = false;
                    movementScript.enabled = true;

                    RovColliders[0].gameObject.layer = LayerMask.NameToLayer("Uninteractable");

                    interactionCount++;
                    missionStatus(interactionCount);
                    Debug.Log(interactionCount);
                }
            }
        }
        else
        {
            //disables display UI for what key to press and ensures it remains off till needed
            ControlPrompt.SetActive(false);
        }
        if (interactionCount >= 2)
        {
            StartCoroutine(LevelCompleteRoutine());
        }
    }

    private bool visCheck()
    {
        Renderer renderer = target1.GetComponent<Renderer>();
        if (renderer != null)
        {
            // Check if the object's color is green
            if (renderer.sharedMaterial.color.Equals(Color.green))
            {
                return true; // Return true if the object is green
            }
        }
        return false;
    }

    void SwitchToCamMode()
    {
        mainCamera.enabled = false;
        camMode.enabled = true;
        isCamModeActive = true;

        RaycastHit hit;
        if (Physics.Raycast(camMode.transform.position, camMode.transform.forward, out hit))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Photograph"))
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    hit.collider.gameObject.layer = LayerMask.NameToLayer("Uninteractable");

                    camMode.enabled = false;
                    mainCamera.enabled = true;
                    isCamModeActive = false;
                    RovColliders[0].gameObject.layer = LayerMask.NameToLayer("Uninteractable");
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
        }
    }

    private IEnumerator LevelCompleteRoutine()
    {
        Debug.Log("Level complete");
        yield return new WaitForSeconds(3); //lets animation play for 3 secnds
        //LevelCompleteScreen.SetActive(true);
        Time.timeScale = 0f; //pauses further action
    }

    private void missionStatus(int interactionCount)
    {
        //mission progress text, changes as the mission progresses\
        switch (interactionCount)
        {
            case 1:
                //Status0.SetActive(false);
                //Status1.SetActive(true);
                break;
            case 2:
                //Status1.SetActive(false);
                //Status2.SetActive(true);
                break;
            case 3:
                //Status2.SetActive(false);
                ///Status3.SetActive(true);
                break;
            default:
                //Status3.SetActive(false);
                //StatusComplete.SetActive(true);
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactionP.position, interactionR);
    }
}