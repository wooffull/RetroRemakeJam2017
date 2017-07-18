using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip healSfx;
    public AudioClip collectMoneySfx;
    public AudioClip denySfx;
    public AudioClip takeDamageSfx;
    public AudioClip jumpSfx;
    public AudioClip shootArrowSfx;
    public AudioClip reaperSpotsPlayerSfx;

    public float denySfxThrottleTime = 0.25f;

    public static AudioManager instance = null;

    private float lastDenySfxStartTime = -1000;
    private AudioSource mainBgm;

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

    void Start()
    {
        mainBgm = gameObject.GetComponent<AudioSource>();
    }

    public void PlayHealSound()
    {
        Play(healSfx);
    }

    public void PlayTakeDamageSound()
    {
        Play(takeDamageSfx);
    }

    public void PlayMoneySound()
    {
        Play(collectMoneySfx);
    }

    public void PlayJumpSound()
    {
        Play(jumpSfx);
    }

    public void PlayDenySound()
    {
        if (Time.time - lastDenySfxStartTime >= denySfxThrottleTime)
        {
            lastDenySfxStartTime = Time.time;
            Play(denySfx);
        }
    }

    public void PlayShootArrowSound()
    {
        Play(shootArrowSfx);
    }

    public void PlayReaperSpotsPlayerSound()
    {
        Play(reaperSpotsPlayerSfx);
        StartCoroutine(DuckMainBgm(reaperSpotsPlayerSfx.length));
    }

    private void Play(AudioClip audioClip)
    {
        AudioSource a = gameObject.AddComponent<AudioSource>();
        a.clip = audioClip;
        a.Play();
    }

    private IEnumerator DuckMainBgm(float duration)
    {
        mainBgm.volume = 0.1f;
        yield return new WaitForSecondsRealtime(duration);
        mainBgm.volume = 1f;
    }
}
