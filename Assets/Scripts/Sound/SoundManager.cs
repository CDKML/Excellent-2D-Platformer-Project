using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    AudioMixer audioMixer;

    [SerializeField]
    AudioMixerGroup audioMixerGroup;

    [SerializeField]
    Text textValue;

    readonly private string masterVolume = "masterVolume";
    readonly private string musicVolume = "musicVolume";
    readonly private string sfxVolume = "sfxVolume";
    readonly private int factor = 20;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetMasterVolume(float sliderValue)
    {
        audioMixer.SetFloat(masterVolume, Mathf.Log10(sliderValue) * factor);
        SetTextValue(sliderValue);
        Save(sliderValue, masterVolume);
    }

    public void SetMusicVolume(float sliderValue)
    {
        audioMixer.SetFloat(musicVolume, Mathf.Log10(sliderValue) * factor);
        SetTextValue(sliderValue);
        Save(sliderValue, musicVolume);
    }

    public void SetSfxVolume(float sliderValue)
    {
        audioMixer.SetFloat(sfxVolume, Mathf.Log10(sliderValue) * factor);
        SetTextValue(sliderValue);
        Save(sliderValue, sfxVolume);
    }

    public void SetTextValue(float sliderValue)
    {
        var vol = sliderValue * 100f;
        textValue.text = vol.ToString("0");
    }

    private void Load()
    {
        SetMasterVolume(PlayerPrefs.GetFloat(masterVolume));
        SetMusicVolume(PlayerPrefs.GetFloat(musicVolume));
        SetSfxVolume(PlayerPrefs.GetFloat(sfxVolume));
    }

    private void Save(float sliderValue, string name)
    {
        if (name == masterVolume)
        {
            PlayerPrefs.SetFloat(masterVolume, sliderValue);
        }
        if (name == musicVolume)
        {
            PlayerPrefs.SetFloat(musicVolume, sliderValue);
        }
        if (name == sfxVolume)
        {
            PlayerPrefs.SetFloat(sfxVolume, sliderValue);
        }
    }
}