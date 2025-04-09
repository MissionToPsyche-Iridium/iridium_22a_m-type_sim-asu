using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIButtonSoundManager : MonoBehaviour
{
    public AudioClip clickClip;
    public AudioSource audioSource;

    private static UIButtonSoundManager instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        HookUpAllButtons();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HookUpAllButtons(); 
    }

    void HookUpAllButtons()
    {
        Button[] buttons = FindObjectsOfType<Button>(true);
        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => PlayClick());
        }
    }

    void PlayClick()
    {
        if (audioSource != null && clickClip != null)
            audioSource.PlayOneShot(clickClip);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
