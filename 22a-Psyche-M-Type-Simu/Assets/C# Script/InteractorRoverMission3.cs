using UnityEngine;

public class InteractorRoverMission3 : InteractorRover
{
    public Animator hduAnimator; // Reference to HDU Animator
    public GameObject hduObject; // Reference to HDU GameObject
    private bool hasDeployedBase = false;
    private bool missionComplete = false;
    public GameObject Status4;
    public GameObject Status5;

    private float lastRightClickTime = 0f;
    private int rightClickCount = 0;
    private float doubleClickThreshold = 0.4f; // seconds between clicks for a double-click


    protected override void Start()
    {
        base.Start();

        // Hide HDU at the start
        if (hduObject != null)
        {
            hduObject.SetActive(false);
        }
        if (Status4 != null)
        { 
            Status4.SetActive(true); 
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
                return;
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

            // --- Normal Drill Site Interaction ---
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
        }
    }

    protected override void missionStatus(int interactionCount)
    {
        float fadeOutDelay = 1.0f;
        float fadeInDelay = 0.5f;

        switch (interactionCount)
        {
            case 1:
                StartCoroutine(FadeTextTransition(Status0, Status1, fadeOutDelay, fadeInDelay));
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

