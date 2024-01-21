using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

public class GameManager : SingleTon<GameManager>
{
    public float time;
    public GameEvent pauseEvent;
    public Player player;
    public HiRil hiril;
    public HaiKen haiken;

    IEnumerator gameStartCo;

    private void Start()
    {
        gameStartCo = GameStartCo();
        GameStart();
        pauseEvent.RegisterListener(() => { GameStop(); });
        pauseEvent.UnregisterListener(() => { GameStart(); });
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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