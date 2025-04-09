using UnityEngine;

public class InteractorRoverMission3 : InteractorRover
{
    public Animator hduAnimator; // Reference to HDU Animator
    public GameObject hduObject; // Reference to HDU GameObject
    private bool hasDeployedBase = false;
    private bool missionComplete = false;
    public GameObject Status4;
    public GameObject Status5;
    public GameObject Status01;
    public GameObject obj1;
    public GameObject obj2;
    public GameObject obj3;
    public GameObject finalObj;

    protected override void Start()
    {
        base.Start();

        obj1.gameObject.layer = LayerMask.NameToLayer("Uninteractable");
        obj2.gameObject.layer = LayerMask.NameToLayer("Uninteractable");
        obj3.gameObject.layer = LayerMask.NameToLayer("Uninteractable");
        finalObj.gameObject.layer = LayerMask.NameToLayer("Uninteractable");

        // Hide HDU at the start
        if (hduObject != null)
        {
            hduObject.SetActive(false);
        }
        if (Status4 != null)
        { 
            Status4.SetActive(true);
            Status01.SetActive(false);
        }
        if (Status5 != null)
        {
            Status5.SetActive(false);
        }
    }

    protected override void Update() {
        // --- Double Right Click Detection ---
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

        // Interaction inputs
        bool ePressed = Input.GetKey(KeyCode.E);
        bool doubleRightClicked = rightClickCount >= 2;

        // --- Interactable Detection ---
        numInteractFound = Physics.OverlapSphereNonAlloc(interactionP.position, interactionR, RovColliders, interactionM);

        if (numInteractFound > 0) {
            var interactable = RovColliders[0].GetComponent<IInteractable>();

            ControlPrompt.SetActive(true);

            // --- Interact with BaseLocation ---
            if (RovColliders[0].gameObject.CompareTag("BaseLocation")) {
                if (!hasDeployedBase && (ePressed || doubleRightClicked)) {
                    DeployBase();
                    RovColliders[0].gameObject.layer = LayerMask.NameToLayer("Uninteractable");
                    ControlPrompt.SetActive(false);

                    // Reset double-click tracking
                    rightClickCount = 0;
                    lastRightClickTime = 0f;
                }
                return; // Prevent interaction count increase
            }

            // --- Interact with FinalSpot ---
            if (RovColliders[0].gameObject.CompareTag("FinalSpot")) {
                if (Status5.activeSelf && !missionComplete && (ePressed || doubleRightClicked)) {
                    missionComplete = true;
                    StartCoroutine(LevelCompleteRoutine());

                    // Reset double-click tracking
                    rightClickCount = 0;
                    lastRightClickTime = 0f;
                }
                return;
            }

            // --- Normal drilling interaction ---
            if (interactable != null && (ePressed || doubleRightClicked)) {
                ControlPrompt.SetActive(false);
                interactable.Interact(this);
                animator.SetTrigger("ActivateDrill");

                ShowDrillCamera();

                RovColliders[0].gameObject.layer = LayerMask.NameToLayer("Uninteractable");

                interactionCount++;

                StartCoroutine(DisableMovementForSeconds(6));

                missionStatus(interactionCount);
                Debug.Log("Interaction Count: " + interactionCount);

                // Reset double-click tracking
                rightClickCount = 0;
                lastRightClickTime = 0f;
            }
        }
        else {
            ControlPrompt.SetActive(false);
        }

        // Unlock the final objective after 3 drill interactions
        if (interactionCount == 3) {
            finalObj.gameObject.layer = LayerMask.NameToLayer("Interactable");
        }
    }


    private void DeployBase()
    {
        hasDeployedBase = true;

        if (hduObject != null)
        {
            hduObject.SetActive(true);
        }

        if (animator != null)
        {
            animator.SetTrigger("DeployBaseTrigger");
        }

        if (hduAnimator != null)
        {
            hduAnimator.SetTrigger("HDUScaleAnimationTrigger");
        }
        if (Status4 != null)
        {
            Status4.SetActive(false);
            Status01.SetActive(true);

        }
        //activates mission objectives
        obj1.gameObject.layer = LayerMask.NameToLayer("Interactable");
        obj2.gameObject.layer = LayerMask.NameToLayer("Interactable");
        obj3.gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    protected override void missionStatus(int interactionCount)
    {
        float fadeOutDelay = 1.0f;
        float fadeInDelay = 0.5f;

        switch (interactionCount)
        {
            case 1:
                StartCoroutine(FadeTextTransition(Status01, Status1, fadeOutDelay, fadeInDelay));
                break;
            case 2:
                StartCoroutine(FadeTextTransition(Status1, Status2, fadeOutDelay, fadeInDelay));
                break;
        }

        // Status5 only appears after Status2 + base setup
        if (interactionCount == 3 && hasDeployedBase)
        {
            StartCoroutine(FadeTextTransition(Status2, Status5, fadeOutDelay, fadeInDelay));

        }
    }
}

