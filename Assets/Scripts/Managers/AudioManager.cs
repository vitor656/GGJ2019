using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sounds{
    COMPUTER,
    KEYS
}

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public AudioClip[] computerSnd;

    public AudioClip[] keysSnd;

    public AudioClip[] koreanVoice;
    public AudioClip[] frenchVoice;
    public AudioClip[] cubanVoice;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(Sounds sound)
    {
        switch(sound)
        {
            case Sounds.COMPUTER:
                audioSource.PlayOneShot(computerSnd[Random.RandomRange(0, computerSnd.Length)]);
            break;
            case Sounds.KEYS:
                audioSource.PlayOneShot(keysSnd[Random.RandomRange(0, keysSnd.Length)]);
            break;
        }
    }

    public void PlayVoice(Nacionalidade nac)
    {
        switch(nac)
        {
            case Nacionalidade.COREIA_NORTE:
                audioSource.PlayOneShot(koreanVoice[Random.RandomRange(0, koreanVoice.Length)]);
            break;
            case Nacionalidade.FRANCA:
                audioSource.PlayOneShot(frenchVoice[Random.RandomRange(0, frenchVoice.Length)]);
            break;
            case Nacionalidade.CUBA:
                audioSource.PlayOneShot(cubanVoice[Random.RandomRange(0, cubanVoice.Length)]);
            break;
        }
    }
}
