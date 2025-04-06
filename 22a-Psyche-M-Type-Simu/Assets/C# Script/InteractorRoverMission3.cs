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

    protected override void Update()
    {
        numInteractFound = Physics.OverlapSphereNonAlloc(interactionP.position, interactionR, RovColliders, interactionM);

        if (numInteractFound > 0)
        {
            var interactable = RovColliders[0].GetComponent<IInteractable>();

            ControlPrompt.SetActive(true);

            // Check if interacting with the base
            if (RovColliders[0].gameObject.CompareTag("BaseLocation"))
            {               
                if (!hasDeployedBase && Input.GetKey(KeyCode.E))
                {                
                    DeployBase();
                    RovColliders[0].gameObject.layer = LayerMask.NameToLayer("Uninteractable");
                    ControlPrompt.SetActive(false);
                }
                return; // Prevent interaction count increase
            }

            // Check if interacting with the final spot
            if (RovColliders[0].gameObject.CompareTag("FinalSpot"))
            {
                if (Status5.activeSelf && !missionComplete && Input.GetKeyDown(KeyCode.E))
                {
                    missionComplete = true; // Mark as interacted
                    StartCoroutine(FadeTextTransition(Status5, StatusComplete, 1.0f, 0.5f));
                    StartCoroutine(LevelCompleteRoutine()); // Mission complete!
                }
                return;
            }

            // Normal drilling interaction
            if (interactable != null && Input.GetKey(KeyCode.E))
            {
                ControlPrompt.SetActive(false);
                interactable.Interact(this);
                animator.SetTrigger("ActivateDrill");

                ShowDrillCamera();

                RovColliders[0].gameObject.layer = LayerMask.NameToLayer("Uninteractable");

                interactionCount++; // Increase count only for drilling spots

                StartCoroutine(DisableMovementForSeconds(6));

                missionStatus(interactionCount);
                Debug.Log("Interaction Count: " + interactionCount);
            }
        }
        else
        {
            ControlPrompt.SetActive(false);
        }

        if (interactionCount == 3)
        {
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

