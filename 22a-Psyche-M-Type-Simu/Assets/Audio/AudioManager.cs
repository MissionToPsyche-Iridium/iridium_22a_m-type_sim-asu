using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keeps music playing across scenes
            GetComponent<AudioSource>().Play(); // Start playing music
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate AudioManagers
        }
    }
}
