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
    [SerializeField] private bool isCamModeActive = false;

    [SerializeField] private GameObject target1;
    [SerializeField] private GameObject target2;
    [SerializeField] private GameObject target3;
    [SerializeField] protected GameObject Status0;
    [SerializeField] protected GameObject Status1;
    [SerializeField] protected GameObject Status2;
    [SerializeField] protected GameObject Status3;
    [SerializeField] protected GameObject LevelCompleteScreen;
    [SerializeField] private GameObject roverCAMmodel;
    [SerializeField] private GameObject mapmaskUI;
    [SerializeField] private GameObject mapoutlineUI;
    [SerializeField] private GameObject ObjectInView;
    [SerializeField] private GameObject ObjectOutView;
    [SerializeField] private GameObject CamFrame;
    [SerializeField] private GameObject MapObject;
    [SerializeField] private GameObject MapTxt;
    private MapFunciton MapScript;
    bool inCamView = false;

    private float lastRightClickTime = 0f;
    private int rightClickCount = 0;
    private float doubleClickThreshold = 0.4f; // max time between two right-clicks in seconds


    //counts the number of interactions the rover has had
    private int interactionCount = 0;
    private void Start()
    {
        animator = GetComponent<Animator>();
        //Status0.SetActive(true);
        mainCamera.enabled = true;
        camMode.enabled = false;

        MapScript = MapObject.GetComponent<MapFunciton>();
        //StartCoroutine(DisableMovementForSeconds(3));
    }

    private void Update() {
        // Detect nearby interactables
        numInteractFound = Physics.OverlapSphereNonAlloc(interactionP.position, interactionR, RovColliders, interactionM);

        // Double right-click detection logic
        if (Input.GetMouseButtonDown(1)) // Right-click = button 1
        {
            float timeSinceLastClick = Time.time - lastRightClickTime;

            if (timeSinceLastClick <= doubleClickThreshold) {
                rightClickCount++;
            }
            else {
                rightClickCount = 1;
            }

            lastRightClickTime = Time.time;
        }

        // Input tracking
        bool ePressed = Input.GetKey(KeyCode.E);
        bool rPressed = Input.GetKey(KeyCode.R);
        bool doubleRightClicked = rightClickCount >= 2;

        if (numInteractFound > 0) {
            var interactable = RovColliders[0].GetComponent<Miss2IInteractable>();

            ControlPrompt.SetActive(true);

            // Interaction with E or double-click
            if (interactable != null && (ePressed || (doubleRightClicked && !isCamModeActive))) {
                ControlPrompt.SetActive(false);
                interactable.Interact(this);

                animator.SetTrigger("CameraActiv");

                mainCamera.enabled = false;
                camMode.enabled = true;
                isCamModeActive = true;

                movementScript.enabled = false;

                // Reset click tracking
                rightClickCount = 0;
                lastRightClickTime = 0f;
            }

            // Exit camera mode with R or double-click (when already in camera mode)
            else if ((rPressed || (doubleRightClicked && isCamModeActive)) && isCamModeActive) {
                bool visibility = visCheck();

                if (visibility) {
                    camMode.enabled = false;
                    mainCamera.enabled = true;
                    isCamModeActive = false;
                    movementScript.enabled = true;

                    RovColliders[0].gameObject.layer = LayerMask.NameToLayer("Uninteractable");

                    interactionCount++;
                    missionStatus(interactionCount);
                    Debug.Log(interactionCount);

                    roverCAMmodel.SetActive(true);

                    // Reset click tracking
                    rightClickCount = 0;
                    lastRightClickTime = 0f;
                }
            }
        }
        else {
            ControlPrompt.SetActive(false);
        }

        if (interactionCount >= 3) {
            LevelCompleteRoutine();
        }

        // UI logic for cam mode
        if (isCamModeActive) {
            MapScript.enabled = false;
            inCamView = UICheck();

            CamFrame.SetActive(true);
            roverCAMmodel.SetActive(false);
            mapmaskUI.SetActive(false);
            mapoutlineUI.SetActive(false);
            MapTxt.SetActive(false);

            if (inCamView) {
                ObjectInView.SetActive(true);
                ObjectOutView.SetActive(false);
            }
            else {
                ObjectOutView.SetActive(true);
                ObjectInView.SetActive(false);
            }
        }
        else {
            MapTxt.SetActive(true);
            MapScript.enabled = true;
            CamFrame.SetActive(false);
            roverCAMmodel.SetActive(true);
            mapmaskUI.SetActive(true);
            mapoutlineUI.SetActive(true);
            ObjectOutView.SetActive(false);
            ObjectInView.SetActive(false);
        }
    }


    private bool UICheck()
    {
        Renderer renderer1 = target1.GetComponent<Renderer>();
        Renderer renderer2 = target2.GetComponent<Renderer>();
        Renderer renderer3 = target3.GetComponent<Renderer>();
        //ObjectCameraDetection script1;
        bool canUse1 = true;
        bool canUse2 = true;
        bool canUse3 = true;
        if (canUse1 == true)
        {
            //check if the object's color is green
            if (renderer1.sharedMaterial.color.Equals(Color.green))
            {
                return true; //return true if the object is green
            }
        }
        if (canUse2 == true)
        {
            //check if the object's color is green
            if (renderer2.sharedMaterial.color.Equals(Color.green))
            {
                return true; // return true if the object is green
            }
        }
        if (canUse3 == true)
        {
            //check if the object's color is green
            if (renderer3.sharedMaterial.color.Equals(Color.green))
            {
                return true; //return true if the object is green
            }
        }
        return false;
    }

    private bool visCheck()
    {
        Renderer renderer1 = target1.GetComponent<Renderer>();
        Renderer renderer2 = target2.GetComponent<Renderer>();
        Renderer renderer3 = target3.GetComponent<Renderer>();
        ObjectCameraDetection script1;
        bool canUse1 = true;
        bool canUse2 = true;
        bool canUse3 = true;
        if (canUse1 == true)
        {
            //check if the object's color is green
            if (renderer1.sharedMaterial.color.Equals(Color.green))
            {
                target1.GetComponent<Renderer>().material.color = Color.blue;
                script1 = target1.GetComponent<ObjectCameraDetection>();
                script1.enabled = false;
                canUse1 = false;

                return true; //return true if the object is green
            }
        }
        if (canUse2 == true)
        {
            //Check if the object's color is green
            if (renderer2.sharedMaterial.color.Equals(Color.green))
            {
                target2.GetComponent<Renderer>().material.color = Color.blue;
                script1 = target2.GetComponent<ObjectCameraDetection>();
                script1.enabled = false;
                canUse2 = false;

                return true; //Return true if the object is green
            }
        }
        if (canUse3 == true)
        {
            //Check if the object's color is green
            if (renderer3.sharedMaterial.color.Equals(Color.green))
            {
                target3.GetComponent<Renderer>().material.color = Color.blue;
                script1 = target3.GetComponent<ObjectCameraDetection>();
                script1.enabled = false;
                canUse3 = false;

                return true; //return true if the object is green
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

    private void LevelCompleteRoutine()
    {
        Debug.Log("Level complete");
        //yield return new WaitForSeconds(1); //lets animation play for 3 secnds
        
        LevelCompleteScreen.SetActive(true);
        Time.timeScale = 0f;
        //pauses further action
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
                
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactionP.position, interactionR);
    }
}