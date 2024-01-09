using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : SingleTon<GameManager>
{
    public Player player;
    public Monster monster;
    public static event Action FinalAttraction;

    public static void StartAttraction()
    {
        Debug.Log("TEST" + FinalAttraction);
        FinalAttraction();
    }
    public static void Subscribe(Action action)
    {
        FinalAttraction += action;
    }

    public static void UnSubscribe(Action action)
    {
        FinalAttraction -= action;
    }
}