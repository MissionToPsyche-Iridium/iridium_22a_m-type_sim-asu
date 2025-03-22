using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioSettings : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        if (masterSlider == null) Debug.LogError("Master slider is not assigned!");
        if (musicSlider == null) Debug.LogError("Music slider is not assigned!");
        if (sfxSlider == null) Debug.LogError("SFX slider is not assigned!");
        if (audioMixer == null) Debug.LogError("AudioMixer is not assigned!");

        // Ensure PlayerPrefs have valid values before loading
        if (!PlayerPrefs.HasKey("MasterVolume")) PlayerPrefs.SetFloat("MasterVolume", 0.75f);
        if (!PlayerPrefs.HasKey("MusicVolume")) PlayerPrefs.SetFloat("MusicVolume", 0.75f);
        if (!PlayerPrefs.HasKey("SFXVolume")) PlayerPrefs.SetFloat("SFXVolume", 0.75f);
        PlayerPrefs.Save();

        // Assign values before adding listeners
        if (masterSlider != null) masterSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("MasterVolume"));
        if (musicSlider != null) musicSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("MusicVolume"));
        if (sfxSlider != null) sfxSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("SFXVolume"));

        // Apply the saved volumes
        ApplyVolume();

        // Add listeners *after* setting values
        if (masterSlider != null) masterSlider.onValueChanged.AddListener(SetMasterVolume);
        if (musicSlider != null) musicSlider.onValueChanged.AddListener(SetMusicVolume);
        if (sfxSlider != null) sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }



    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    void ApplyVolume()
    {
        SetMasterVolume(masterSlider.value);
        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);
    }
}
