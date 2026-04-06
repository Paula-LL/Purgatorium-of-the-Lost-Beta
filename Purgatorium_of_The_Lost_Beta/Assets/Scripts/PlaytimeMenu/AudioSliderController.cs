using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.Rendering.DebugUI;

public class AudioSliderController : MonoBehaviour
{
    [SerializeField] private AudioMixer Mixer;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private AudioMixMode mixMode;

    public void OnChangeSlider(float value) {
        valueText.SetText($"{value:N4}");

        switch ( mixMode )
        {
            case AudioMixMode.LogarithmicMixerVolume:
                Mixer.SetFloat("Volume", Mathf.Log10(value) * 20);
                PlayerPrefs.SetFloat("Volume", value);
                PlayerPrefs.Save();
                break;

        }
    }

    public void OnChangeSliderSFX(float sfxVal)
    {
        valueText.SetText($"{sfxVal:N4}");

        switch (mixMode)
        {
            case AudioMixMode.LogarithmicMixerVolume:
                Mixer.SetFloat("SFXVolume", Mathf.Log10(sfxVal) * 20);
                PlayerPrefs.SetFloat("SFXVolume", sfxVal);
                PlayerPrefs.Save();
                break;

        }
    }

    void Start()
    {
        Mixer.SetFloat("Volume", Mathf.Log10(PlayerPrefs.GetFloat("Volume", 1)*20));
        Mixer.SetFloat("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume", 1) * 20));
    }


    public enum AudioMixMode { 
        LinearAudioSourceVolume, 
        LinearMixerVolume, 
        LogarithmicMixerVolume
    }
}
