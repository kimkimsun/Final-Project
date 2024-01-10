using CustomInterface;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FinalEvent : ScriptableObject, IEventable
{
    public List<GameEventListener> gameEventListeners = new List<GameEventListener>();

    public void Raise()
    {
        foreach(GameEventListener listener in gameEventListeners)
        {
            listener.OnEvent();
        }
    }
    public void RegisterListener(object listener)
    {
        gameEventListeners.Add((GameEventListener)listener);
    }

    public void UnregisterListener(object listener)
    {
        gameEventListeners.Remove((GameEventListener)listener);
    }

}
