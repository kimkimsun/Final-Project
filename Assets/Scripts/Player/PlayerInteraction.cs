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
    private bool isStop;
    

    private void Start()
    {
        player = GetComponent<Player>();
    }
    void Update()
    {
        Stop();
        SwitchItem();
        OpenInven();
        CurEauipItem();
        HairPinCount();

    }

    public void Stop()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isStop = false;
            if (UIManager.Instance.UIStack.Count > 0)
            {
                UIManager.Instance.UIStack.Pop().GameObject().SetActive(false);

            }
            else if (UIManager.Instance.UIStack.Count <= 0)
            {
                isStop = true;
                pause.Raise();

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

    public void CurEauipItem()
    {

        if (player.EquipInven.gameObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Debug.Log(slotIndexNum);
                slotIndexNum--;
                if (slotIndexNum == -1)
                    slotIndexNum = player.EquipInven.EiSlots.Length - 1;
                player.EquipInven.IndexSlot(slotIndexNum);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (slotIndexNum == player.EquipInven.EiSlots.Length)
                    slotIndexNum = 1;
                player.EquipInven.IndexSlot(slotIndexNum);
                slotIndexNum++;
            }
        }
        else
            return;
    }

    public void HairPinCount()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Q����");
            player.portableInven.gameObject.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.Q))
            player.portableInven.gameObject.SetActive(false);
    }
}
