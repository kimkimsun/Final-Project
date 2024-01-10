using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class Follower : MonoBehaviour
{
    public Pause pause;
    public UnityEvent respons;

    public void Stop()
    {

    }
    private void OnEnable()
    {
        pause.Subscribe(this);
    }

    private void OnDisable()
    {
        pause.Subscribecancel(this);
    }
}
