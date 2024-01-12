using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    Player player;

    private void Start()
    {
        player = GetComponent<Player>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (UIManager.Instance.UIStack.Count > 0)
            {
                UIManager.Instance.UIStack.Pop().GameObject().SetActive(false);

            }
            else if(UIManager.Instance.UIStack.Count <= 0)
            {
                //Debug.Log(pause.followers.Count);
                //pause.Raise();
            }
                
        }

        if (player.EquipInven.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Return))
            player.EquipInven.SwitchItem();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Q´©¸§");
            player.portableInven.gameObject.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Q))
            player.portableInven.gameObject.SetActive(false);
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!player.EquipInven.gameObject.activeSelf)
            {
                player.EquipInven.gameObject.SetActive(true);
                player.slotIndexNum = 4;
                player.EquipInven.IndexSlot(player.slotIndexNum);
                UIManager.Instance.UIStack.Push(player.EquipInven);
            }
            else
                return;
        }
        if (player.EquipInven.gameObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Debug.Log(player.slotIndexNum);
                player.slotIndexNum--;
                if (player.slotIndexNum == -1)
                    player.slotIndexNum = player.EquipInven.EiSlots.Length - 1;
                player.EquipInven.IndexSlot(player.slotIndexNum);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                player.slotIndexNum++;
                if (player.slotIndexNum == player.EquipInven.EiSlots.Length)
                    player.slotIndexNum = 1;
                player.EquipInven.IndexSlot(player.slotIndexNum);
            }
        }
        else
            return;
    }
}
