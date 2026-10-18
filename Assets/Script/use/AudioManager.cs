using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource audioSource;
    public AudioSource backgroundaudioSource;
    public AudioClip clipDie;
    public AudioClip clipVictory;
    public AudioClip GetCoin;
    public AudioClip backgroundClip;
    public AudioClip GameBonus;
    private void Awake()
    {
        instance=this;
        backgroundaudioSource.clip = backgroundClip; 
        backgroundaudioSource.loop = true;
        backgroundaudioSource.Play();
    }
    void Start()
    {
        
    }

    public void PlaySfx(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
