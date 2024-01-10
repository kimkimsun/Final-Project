using CustomInterface;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FinalEvent : ScriptableObject, IEventable
{
    public List<ISubscribeable> gameEventListeners = new List<ISubscribeable>();

    public void Raise()
    {
        foreach(ISubscribeable listener in gameEventListeners)
        {
            listener.OnEvent();
        }
    }
    public void RegisterListener(ISubscribeable listener)
    {
        gameEventListeners.Add(listener);
    }

    public void UnregisterListener(ISubscribeable listener)
    {
        gameEventListeners.Remove(listener);
    }

}
