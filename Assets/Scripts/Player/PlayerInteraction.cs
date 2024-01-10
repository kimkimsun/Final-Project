using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    Player player;
    [SerializeField]Pause pause;

    private void Start()
    {
        player = GetComponent<Player>();
    }
    void Update()
    {
        #region 플레이어 상호작용 키
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (UIManager.Instance.UIStack.Count > 0)
            {
                UIManager.Instance.UIStack.Pop().GameObject().SetActive(false);

            }
            else if(UIManager.Instance.UIStack.Count <= 0)
            {
                Debug.Log(pause.followers.Count);
                pause.Raise();
            }
                
        }

        if (player.inven.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            player.inven.SwitchItem();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!player.inven.gameObject.activeSelf)
            {
                player.inven.gameObject.SetActive(true);
                player.slotIndexNum = 4;
                player.inven.IndexSlot(player.slotIndexNum);
                UIManager.Instance.UIStack.Push(player.inven);
            }
            else
                return;
        }
        if (player.inven.gameObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Debug.Log(player.slotIndexNum);
                player.slotIndexNum--;
                if (player.slotIndexNum == -1)
                    player.slotIndexNum = player.inven.EquipQuickSlot.Length - 1;
                player.inven.IndexSlot(player.slotIndexNum);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                player.slotIndexNum++;
                if (player.slotIndexNum == player.inven.EquipQuickSlot.Length)
                    player.slotIndexNum = 1;
                player.inven.IndexSlot(player.slotIndexNum);
            }
        }
        else
            return;
        #endregion
    }
}
