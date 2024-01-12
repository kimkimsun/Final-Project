using CustomInterface;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    public Action eventAction;
    public void Raise()
    {
        eventAction();
    }
    public void RegisterListener(Action listener)
    {
        eventAction += listener;
    }
    public void UnregisterListener(Action listener)
    {
        eventAction -= listener;
    }
}
