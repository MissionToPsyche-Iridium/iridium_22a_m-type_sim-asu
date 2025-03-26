using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public AudioMixer audioMixer; 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 

            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.Play(); 



            ApplySavedVolumes();
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    public void ApplySavedVolumes()
    {
        float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        float savedMasterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0.75f);

        audioMixer.SetFloat("MusicVolume", Mathf.Log10(savedMusicVolume) * 20);
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(savedMasterVolume) * 20);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(savedSFXVolume) * 20);
    }
}
