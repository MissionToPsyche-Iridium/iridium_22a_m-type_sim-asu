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
    [SerializeField] protected Transform interactionP;
    [SerializeField] protected float interactionR = 0.5f;
    [SerializeField] protected LayerMask interactionM;
    [SerializeField] protected GameObject Status0;
    [SerializeField] protected GameObject Status1;
    [SerializeField] protected GameObject Status2;
    [SerializeField] protected GameObject Status3;
    [SerializeField] protected GameObject StatusComplete;
    [SerializeField] protected GameObject LevelCompleteScreen;
    [SerializeField] protected Camera drillCamera; // Reference to the drill camera
    [SerializeField] protected RawImage drillCameraUI; // Reference to the UI panel
    [SerializeField] protected GameObject ControlPrompt;

    protected readonly Collider[] RovColliders = new Collider[3];
    [SerializeField] protected int numInteractFound;
    [SerializeField] protected MonoBehaviour movementScript;

    //counts the number of interactions the rover has had
    protected int interactionCount = 0;

    //Sound effect components
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioClip interactSound;

    protected virtual void Start()
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

    protected void ShowDrillCamera()
    {
        if (drillCamera != null && drillCameraUI != null)
        {
            drillCamera.enabled = true; // Enable the drill camera
            drillCameraUI.gameObject.SetActive(true); // Show the UI panel
        }
    }

    protected void HideDrillCamera()
    {
        if (drillCamera != null && drillCameraUI != null)
        {
            drillCamera.enabled = false; // Disable the drill camera
            drillCameraUI.gameObject.SetActive(false); // Hide the UI panel
        }
    }

    protected virtual void Update()
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

                //Play interaction sound
                if (audioSource != null && interactSound != null)
                {
                    audioSource.PlayOneShot(interactSound);
                }

                // Show the drill camera view
                ShowDrillCamera();

                RovColliders[0].gameObject.layer = LayerMask.NameToLayer("Uninteractable");

                interactionCount++;

                StartCoroutine(DisableMovementForSeconds(6));
                
                missionStatus(interactionCount);
                Debug.Log(interactionCount);
                if (interactionCount >= 4)
                {
                    StartCoroutine(LevelCompleteRoutine());
                }
            }
        }
        else
        {
            ControlPrompt.SetActive(false);
        }
    }

    protected IEnumerator DisableMovementForSeconds(float seconds)
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

    protected virtual IEnumerator LevelCompleteRoutine()
    {
        Debug.Log("Level complete"); 
        yield return new WaitForSeconds(6); //lets animation play for 3 secnds
        LevelCompleteScreen.SetActive(true); 
        Time.timeScale = 0f; //pauses further action
    }

    protected virtual void missionStatus(int interactionCount)
    {

        float fadeOutDelay = 1.0f;
        float fadeInDelay = 0.5f;

        //mission progress text, changes as the mission progresses\
        switch (interactionCount)
        {
            case 1:
                //Status0.SetActive(false);
                //Status1.SetActive(true);
                StartCoroutine(FadeTextTransition(Status0, Status1, fadeOutDelay, fadeInDelay));
                break;
            case 2:
                //Status1.SetActive(false);
                //Status2.SetActive(true);
                StartCoroutine(FadeTextTransition(Status1, Status2, fadeOutDelay, fadeInDelay));
                break;
            case 3:
                //Status2.SetActive(false);
                //Status3.SetActive(true);
                StartCoroutine(FadeTextTransition(Status2, Status3, fadeOutDelay, fadeInDelay));
                break;
            default:
                //Status3.SetActive(false);
                //StatusComplete.SetActive(true);
                StartCoroutine(FadeTextTransition(Status3, StatusComplete, fadeOutDelay, fadeInDelay));
                break;
        }
    }

    protected IEnumerator FadeTextTransition(GameObject oldText, GameObject newText, float fadeOutDelay = 0.5f, float fadeInDelay = 0.5f) {
        CanvasGroup oldCanvas = oldText.GetComponent<CanvasGroup>();
        CanvasGroup newCanvas = newText.GetComponent<CanvasGroup>();

        if (oldCanvas == null) oldCanvas = oldText.AddComponent<CanvasGroup>();
        if (newCanvas == null) newCanvas = newText.AddComponent<CanvasGroup>();

        float duration = 1.0f; // Duration of fade
        float elapsedTime = 0f;

        // Ensure old text is visible at start
        oldCanvas.alpha = 1f;
        newCanvas.alpha = 0f;
        newText.SetActive(true);

        // delay to allow rover to mine before update
        yield return new WaitForSeconds(fadeOutDelay);

        // Fade out old text & fade in new text
        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            float alpha = 1 - (elapsedTime / duration);
            oldCanvas.alpha = alpha;
            yield return null;
        }

        oldCanvas.alpha = 0f;
        oldText.SetActive(false); // Deactivate the old text after fading out

        yield return new WaitForSeconds(fadeInDelay);

        elapsedTime = 0f;

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            float alpha = elapsedTime / duration;
            newCanvas.alpha = alpha;
            yield return null;
        }

        newCanvas.alpha = 1f;
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactionP.position, interactionR);
    }
}
