using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using CustomInterface;

public class PauseObject : MonoBehaviour, ISubscribeable
{
    public Pause pause;
    public UnityEvent respons;
    public void OnEvent()
    {
        throw new NotImplementedException();
    }

    private void OnEnable()
    {
        pause.RegisterListener(this);
    }

    private void OnDisable()
    {
        pause.UnregisterListener(this);
    }

}
