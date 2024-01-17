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
        SwitchItem();
        OpenInven();
        CurEquipItem();
        UseFlash();
    }
    public void UseFlash()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log(player.equipInven.Pocket.item.itemStrategy);
            player.equipInven.Pocket.item.Use();
        }
    }
    public void Stop()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (UIManager.Instance.UIStack.Count > 0)
            {
                UIManager.Instance.UIStack.Pop().GameObject().SetActive(false);

            }
            else if (UIManager.Instance.UIStack.Count <= 0)
            {
                pause.Raise();
                Debug.Log("일시정지 누름");
            }
        }
    }
    public void SwitchItem()
    {
        if (player.EquipInven.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Return))
            player.EquipInven.SwitchItem();
    }

    public void OpenInven()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!player.EquipInven.gameObject.activeSelf)
            {
                player.EquipInven.gameObject.SetActive(true);
                slotIndexNum = 3;
                player.EquipInven.IndexSlot(slotIndexNum);
                UIManager.Instance.UIStack.Push(player.EquipInven);
            }
            else
                return;
        }
    }

    public void CurEquipItem()
    {

        if (player.EquipInven.gameObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                slotIndexNum--;
                if (slotIndexNum == -1)
                    slotIndexNum = player.EquipInven.EiSlots.Length - 1;
                player.EquipInven.IndexSlot(slotIndexNum);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                slotIndexNum++;
                if (slotIndexNum == player.EquipInven.EiSlots.Length)
                    slotIndexNum = 0;
                player.EquipInven.IndexSlot(slotIndexNum);
            }
        }
        else
            return;
    }
}