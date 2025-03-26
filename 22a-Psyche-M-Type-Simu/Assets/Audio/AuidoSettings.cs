using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioSettings : MonoBehaviour
{
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {

        if (AudioManager.Instance == null)
        {
            Debug.LogError("AudioManager is missing in the scene!");
            return;
        }



        if (masterSlider == null) masterSlider = GameObject.Find("MasterSlider")?.GetComponent<Slider>();
        if (musicSlider == null) musicSlider = GameObject.Find("MusicSlider")?.GetComponent<Slider>();
        if (sfxSlider == null) sfxSlider = GameObject.Find("SFXSlider")?.GetComponent<Slider>();


        if (masterSlider == null) Debug.LogError("Master slider not found in scene!");
        if (musicSlider == null) Debug.LogError("Music slider not found in scene!");
        if (sfxSlider == null) Debug.LogError("SFX slider not found in scene!");



        if (masterSlider != null) masterSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("MasterVolume", 0.75f));
        if (musicSlider != null) musicSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("MusicVolume", 0.75f));
        if (sfxSlider != null) sfxSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("SFXVolume", 0.75f));


        if (masterSlider != null) masterSlider.onValueChanged.AddListener(SetMasterVolume);
        if (musicSlider != null) musicSlider.onValueChanged.AddListener(SetMusicVolume);
        if (sfxSlider != null) sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMasterVolume(float volume)
    {
        AudioManager.Instance.audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        AudioManager.Instance.audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        AudioManager.Instance.audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}
