using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

public class GameManager : SingleTon<GameManager>
{
    private bool isStart;
    public float time;

    public GameEvent pauseEvent;
    public Player player;
    public HiRil hiril;
    public HaiKen haiken;
    IEnumerator gameStartCo;

    
    public bool IsStart
    {
        get => isStart;
        set
        {
            isStart = value;
            if (isStart)
            {
                this.enabled = true;
                StartCoroutine(gameStartCo);
            }
            else if (!isStart)
            {
                this.enabled = false;
                StopCoroutine(gameStartCo);
            }
        }
    }
    private void Start()
    {
        gameStartCo = GameStartCo();
        GameStart();
        Debug.Log("게임시작");
        Debug.Log(Save.Instance.fileIndex);
    }
    private void OnEnable()
    {
        pauseEvent.RegisterListener(() => { IsStart = false; });
        pauseEvent.UnregisterListener(() => { IsStart = true; });
    }
    private void OnDisable()
    {
        pauseEvent.RegisterListener(() => { IsStart = true; });
        pauseEvent.UnregisterListener(() => { IsStart = false; });
    }
    public void GameStart()
    {
        StartCoroutine(gameStartCo);
    }

    public void GameStop()
    {
        StopCoroutine(gameStartCo);
    }
    IEnumerator GameStartCo()
    {
        while(true)
        {
            time += Time.deltaTime;
            UIManager.Instance.uiSettingText.text = time.ToString("N2");
            yield return null;
        }
    }
}