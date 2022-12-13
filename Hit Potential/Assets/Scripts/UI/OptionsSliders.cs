using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsSliders : MonoBehaviour
{
    private SoundManager sm;

    private void Start()
    {
        sm = FindObjectOfType<SoundManager>();
    }

    public void MasterSoundChange(float sliderValue)
    {
        sm.SetMasterVolume(sliderValue);
    }

    public void MusicSoundChange(float sliderValue)
    {
        sm.SetMusicVolume(sliderValue);
    }

    public void SFXSoundChange(float sliderValue)
    {
        sm.SetSFXVolume(sliderValue);
    }
}
