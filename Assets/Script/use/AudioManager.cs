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

    void Update()
    {
        
    }
}
