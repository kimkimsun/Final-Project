using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundComponent : MonoBehaviour
{
    public AudioSource audioSource;

    public void Play(AudioClip clip)
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();

    }

    private void Update()
    {
        if (audioSource.isPlaying == false)
            SoundManager.Instance.ReturnPool(this);
    }
}

