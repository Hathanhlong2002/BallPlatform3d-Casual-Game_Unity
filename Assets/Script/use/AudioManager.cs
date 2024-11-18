using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : Singleton<AudioManager>
{
    public AudioSource audioSource;
    public AudioSource backgroundaudioSource;
    public AudioClip clipDie;
    public AudioClip clipVictory;
    public AudioClip GetCoin;
    public AudioClip backgroundClip;
    public AudioClip GameBonus;

    public Slider volumeSlider;
   
    private void Start()
    {
        backgroundaudioSource.clip = backgroundClip;
        backgroundaudioSource.loop = true;
        backgroundaudioSource.Play();
        ChangeVolume();
        volumeSlider.onValueChanged.AddListener(delegate { SettingVolume(); });
    }
    public void ChangeVolume()
    {
        audioSource.volume=PlayerPrefs.GetFloat("musicVolume");
        backgroundaudioSource.volume = PlayerPrefs.GetFloat("musicVolume");
    }
    public void SettingVolume()
    {
        audioSource.volume = volumeSlider.value;
        backgroundaudioSource.volume = audioSource.volume;
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
        PlayerPrefs.Save();

    }
}
