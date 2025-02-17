using UnityEngine;

public class InteractorRoverMission3 : InteractorRover
{
    public Animator hduAnimator; // Reference to HDU Animator
    private bool hasDeployedBase = false;

    protected override void Update()
    {
        base.Update();

        if (!hasDeployedBase && numInteractFound > 0 && RovColliders[0].gameObject.CompareTag("BaseLocation"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                hasDeployedBase = true;
                animator.SetTrigger("DeployBaseTrigger");

                if (hduAnimator != null)
                {
                    hduAnimator.SetTrigger("HDUScaleAnimationTrigger");
                }

                Invoke(nameof(ResetDeployBase), 5f);
            }
        }
    }

    private void ResetDeployBase()
    {
        hasDeployedBase = false;
    }
}

