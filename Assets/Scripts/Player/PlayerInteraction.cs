using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    Player player;
    public GameEvent pause;
    private int slotIndexNum;
    private void Start()
    {
        player = GetComponent<Player>();
    }
    void Update()
    {
        Stop();
        UseFlash();
    }
    public void UseFlash()
    {
        if (Input.GetKeyDown(KeyCode.F))
            player.UseFlash();
    }
    public void Stop()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            if (UIManager.Instance.UIStack.Count > 0)
            {
                UIManager.Instance.UIStack.Pop().GameObject().SetActive(false);

            }
            else if (UIManager.Instance.UIStack.Count <= 0)
            {
                pause.Raise();
                UIManager.Instance.settingBox.SetActive(true);
                UIManager.Instance.UIStack.Push(UIManager.Instance.settingBox);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }

    }
}