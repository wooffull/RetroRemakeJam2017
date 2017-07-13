using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip healSfx;
    public AudioClip takeDamageSfx;

    public static AudioManager instance = null;

    void Awake()
    {
        // Check if instance already exists
        if (instance == null)
        {
            instance = this;
        }

        // If it already exists and it's not this, then destroy this (to enforce our Singleton pattern)
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // This won't be destroyed when reloading scenes
        DontDestroyOnLoad(gameObject);
    }

    public void PlayHealSound()
    {
        AudioSource a = gameObject.AddComponent<AudioSource>();
        a.clip = healSfx;
        a.Play();
    }

    public void PlayTakeDamageSound()
    {
        AudioSource a = gameObject.AddComponent<AudioSource>();
        a.clip = takeDamageSfx;
        a.Play();
    }
}
