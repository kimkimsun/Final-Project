using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FinalEvent : ScriptableObject
{
    public List<GameEventListener> gameEventListeners = new List<GameEventListener>();

    public void Raise()
    {
        foreach(GameEventListener listener in gameEventListeners)
        {
            listener.onEventRaised();
        }
    }
    public void RegisterListener(GameEventListener listener)
    {
        gameEventListeners.Add(listener);
    }
    public void UnregisterListener(GameEventListener listener)
    {
        gameEventListeners.Remove(listener);
    }

}
