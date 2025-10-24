using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clip")]
    public AudioClip background;
    public AudioClip death;
    public AudioClip hit;


    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();

    }

    public bool PlaySFX(AudioClip clip)
    {
        if (clip == null || SFXSource == null) return false;
        SFXSource.clip = clip;
        SFXSource.Play();
        return true;
    }
}
