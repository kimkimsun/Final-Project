using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : ISubscribeable
{
    public FinalEvent Event;
    public UnityEvent Response;
    public void OnEvent()
    {
        Response.Invoke();
    }
    private void OnEnable()
    {
        Event.RegisterListener(this);
    }
    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }
}