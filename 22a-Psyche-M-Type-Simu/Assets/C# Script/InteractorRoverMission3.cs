using UnityEngine;

public class InteractorRoverMission3 : InteractorRover
{
    public Animator hduAnimator; // Reference to HDU Animator
    public GameObject hduObject; // Reference to HDU GameObject
    private bool hasDeployedBase = false;

    protected override void Start()
    {
        base.Start();

        // Hide HDU at the start
        if (hduObject != null)
        {
            hduObject.SetActive(false);
        }
    }

    protected override void Update()
    {
        numInteractFound = Physics.OverlapSphereNonAlloc(interactionP.position, interactionR, RovColliders, interactionM);

        if (numInteractFound > 0)
        {
            var interactable = RovColliders[0].GetComponent<IInteractable>();

            // **Handle Base Deployment Separately**
            if (!hasDeployedBase && RovColliders[0].gameObject.CompareTag("BaseLocation"))
            {
                ControlPrompt.SetActive(true);

                if (Input.GetKey(KeyCode.E))
                {
                    DeployBase();
                    ControlPrompt.SetActive(false);
                }
                return; // Prevent further drilling interaction processing
            }

            // **Retain Drill Interaction from Parent**
            base.Update();
        }
        else
        {
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

        Invoke(nameof(ResetDeployBase), 5f);
    }


    private void ResetDeployBase()
    {
        hasDeployedBase = false;
    }
}

