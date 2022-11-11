using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioMixer mixer;

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    private float musicVolume = 1f;
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMasterVolume(float sliderValue)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
    }

    public void SetMusicVolume(float sliderValue)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }

    public void SetSFXVolume(float sliderValue)
    {
        mixer.SetFloat("sfxVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("sfxVolume", sliderValue);
    }
}
