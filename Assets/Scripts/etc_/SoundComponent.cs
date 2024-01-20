using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundComponent : MonoBehaviour
{
    public AudioSource audioSource;

    public void Play(AudioClip clip , bool isloop)
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.loop = isloop;
        audioSource.Play();

        if (isloop)
            audioSource.volume = 0.1f;
    }

    private void Update()
    {
        if ( audioSource.isPlaying == false)
            SoundManager.Instance.ReturnPool(this);

    }
}
