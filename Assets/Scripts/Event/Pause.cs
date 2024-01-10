using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Properties;
using CustomInterface;

[CreateAssetMenu]
public class Pause : ScriptableObject, IEventable
{
    public static event Action allstop;
    private List<PauseObject> followers = new List<PauseObject>();
    public void Raise()
    {
        foreach (PauseObject follower in followers)
        {
            follower.OnEvent();
        }
    }

    public void RegisterListener(object follower)
    {
        followers.Add((PauseObject)follower);
    }

    public void UnregisterListener(object follower)
    {
        followers.Remove((PauseObject)follower);
    }


}
