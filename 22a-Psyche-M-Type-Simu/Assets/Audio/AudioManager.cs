using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public AudioMixer audioMixer; // Reference to the AudioMixer

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keeps music playing across scenes

            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.Play(); // Start playing music

            // Ensure AudioMixer is updated
            float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(savedMusicVolume) * 20);
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate AudioManagers
        }
    }

}
