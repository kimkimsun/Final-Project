using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipItemInventory : Inventory
{
    [SerializeField] private EquipItemSlot[] eiSlots;
    [SerializeField] private EquipItemSlot equipSlot;
    [SerializeField] private EquipItemSlot pocket;
    [SerializeField] private EquipItemSlot tempSlot;
    [SerializeField] private Image textCoverImage;
    [SerializeField] private EquipItemSlot flashLightPocket;
    [SerializeField] private int index;
    public int Index { get => index; }

    public EquipItemSlot Pocket
    {
        get => pocket;
        set => pocket = value;
    }
    public EquipItemSlot EquipSlot
    {
        get => equipSlot;
        set => equipSlot = value;
    }
    public EquipItemSlot[] EiSlots
    { 
        get => eiSlots; set => eiSlots = value; 
    }
    public override void AddItem(Item item)
    {
        if (item.itemStrategy is FinalKeyItemStrategy)
        {
            Destroy(item);
            GameManager.Instance.player.FinalKey++;
        }

        else if (equipSlot.item == null)
        {
            equipSlot.item = item;
            equipSlot.itemImage.color = new Color(1, 1, 1, 1);
            equipSlot.itemImage.sprite = item.sprite;
            item.Use();
            item.gameObject.SetActive(false);
        }
        else
        {
            for (int i = eiSlots.Length - 1; i >= 0; i--)
            {
                if (eiSlots[i].item == null)
                {
                    eiSlots[i].SetItem(item);
                    break;
                }
            }
        }
    }
    public void IndexSlot(int index)
    {
        this.index = index;
        for (int i = 0; i < eiSlots.Length; i++)
        {
            if (i == index)
            {
                eiSlots[i].transform.GetChild(1).gameObject.SetActive(true);
                if (eiSlots[i].item != null && GameManager.Instance.player.equipInven.gameObject.activeSelf)
                {
                    textCoverImage.gameObject.SetActive(true);
                    textCoverImage.GetComponentInChildren<TextMeshProUGUI>().text = eiSlots[i].item.ExplanationText;
                }
                else
                    textCoverImage.gameObject.SetActive(false);
            }
            else
                eiSlots[i].transform.GetChild(1).gameObject.SetActive(false);
        }
    }
    public void SwitchItem()
    {
        if (eiSlots[index].item != null)
        {
            equipSlot.item.Exit();
            tempSlot.item = eiSlots[index].item;
            tempSlot.itemImage.sprite = eiSlots[index].itemImage.sprite;

            eiSlots[index].item = equipSlot.item;
            eiSlots[index].itemImage.sprite = equipSlot.itemImage.sprite;

            equipSlot.item = tempSlot.item;
            equipSlot.itemImage.sprite = tempSlot.itemImage.sprite;

            tempSlot.item = null;
            tempSlot.itemImage.sprite = null;
            equipSlot.item.Use();
        }
        else
            return;
    }
}