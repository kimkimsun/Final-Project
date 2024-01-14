using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingleTon<SoundManager>
{
    public GameObject SoundPrefab;


    public void Play(AudioClip clip, Transform target = null)
    {
        GameObject temp = Instantiate(SoundPrefab, target);
        temp.GetComponent<SoundComponent>().Play(clip)
    }
}
