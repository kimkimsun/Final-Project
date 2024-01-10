using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Properties;
using CustomInterface;

[CreateAssetMenu]
public class Pause : ScriptableObject, IEventable
{
    public List<ISubscribeable> followers = new List<ISubscribeable>();
    public void Raise()
    {
        foreach (ISubscribeable follower in followers)
        {
            follower.OnEvent();
        }
    }

    public void RegisterListener(ISubscribeable follower)
    {
        followers.Add( follower);
    }

    public void UnregisterListener(ISubscribeable follower)
    {
        followers.Remove(follower);
    }


}
