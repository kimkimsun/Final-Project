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
    [SerializeField] private EquipItemSlot tempSlot;
    [SerializeField] private Image textCoverImage;
    [SerializeField] private int index;
    public int Index { get => index; }
    private void Start()
    {
        eiSlots = new EquipItemSlot[1];
    }
    public override void AddItem(Item item)
    {
        if (equipSlot.Item == null)
        {
            equipSlot.Item = item;
            item.Use();
            item.gameObject.SetActive(false);
        }
        else
        {
            for (int i = eiSlots.Length - 1; i >= 1; i--)
            {
                if (eiSlots[i].Item == null)
                    eiSlots[i].SetItem(item);
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
                eiSlots[i].ItemImage.color = Color.yellow;
                if (eiSlots[i].Item != null && GameManager.Instance.player.Inven.gameObject.activeSelf)
                {
                    textCoverImage.gameObject.SetActive(true);
                    textCoverImage.GetComponentInChildren<TextMeshProUGUI>().text = eiSlots[i].Item.ExplanationText;
                }
            }
            else
            {
                eiSlots[i].ItemImage.color = Color.red;
                textCoverImage.gameObject.SetActive(false);
            }
        }
    }
    public void SwitchItem()
    {
        if (equipSlot.Item != null)
            equipSlot.Item.Exit();
        if (eiSlots[index].Item == null)
            return;

        tempSlot.Item = eiSlots[index].Item;
        tempSlot.ItemSprite = eiSlots[index].ItemSprite;

        eiSlots[index].Item = equipSlot.Item;
        eiSlots[index].ItemSprite = equipSlot.ItemSprite;

        equipSlot.Item = tempSlot.Item;
        equipSlot.ItemSprite = tempSlot.ItemSprite;

        tempSlot.Item = null;
        tempSlot.ItemSprite = null;
        equipSlot.Item.Use();
    }
}