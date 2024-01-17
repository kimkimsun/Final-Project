using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

public class GameManager : SingleTon<GameManager>
{
    public Player player;
    public HiRil hiril;
    public HaiKen haiken;
    public CinemachineVirtualCamera[] endingCamera = new CinemachineVirtualCamera[2];

    public void HirilEnding()
    {
        endingCamera[0].Priority = 11;
    }
    public void HaikenEnding()
    {
        endingCamera[1].Priority = 11;
    }
}