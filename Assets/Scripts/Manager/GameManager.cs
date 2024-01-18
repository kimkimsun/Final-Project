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
    public CinemachineVirtualCamera hirilCamera;
    public CinemachineVirtualCamera haikenCamera;
    public void HirilEnding()
    {
        hirilCamera.Priority = 11;
    }
    public void HaikenEnding()
    {
        haikenCamera.Priority = 11;
    }
    public void BadEnding()
    {
        Debug.Log("배드엔딩");
    }
}