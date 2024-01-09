using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PauseManager : MonoBehaviour
{
    public static event Action allstop;
    private List<Follower> followers = new List<Follower>();

    public void AllStop()
    {
        foreach (Follower follower in followers)
        {
            follower.Stop();
        }
    }
    public void Subscribe(Follower follower)
    {
        followers.Add(follower);
    }

    public void Subscribecancel(Follower follower)
    {
        followers.Remove(follower);
    }

}
