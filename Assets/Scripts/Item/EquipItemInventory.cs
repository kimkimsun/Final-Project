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

    public EquipItemSlot[] EiSlots
    { 
        get => eiSlots; set => eiSlots = value; 
    }
    private void Start()
    {
        eiSlots = new EquipItemSlot[4];
    }
    public override void AddItem(Item item)
    {
        if (equipSlot.item == null)
        {
            equipSlot.item = item;
            equipSlot.itemImage.sprite = item.sprite;
            item.Use();
            item.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("슬롯 안비었니");
            for (int i = eiSlots.Length - 1; i >= 0; i--)
            {
                if (eiSlots[i].item == null)
                {
                    Debug.Log("슬롯 안비었니222");
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
                eiSlots[i].itemImage.color = Color.yellow;
                if (eiSlots[i].item != null && GameManager.Instance.player.equipInven.gameObject.activeSelf)
                {
                    textCoverImage.gameObject.SetActive(true);
                    textCoverImage.GetComponentInChildren<TextMeshProUGUI>().text = eiSlots[i].item.ExplanationText;
                }
            }
            else
            {
                Debug.Log(eiSlots[i].itemImage.name);
                eiSlots[i].itemImage.GetComponent<Image>().color = Color.blue;
                textCoverImage.gameObject.SetActive(false);
            }
        }
    }
    public void SwitchItem()
    {
        if (equipSlot.item != null)
            equipSlot.item.Exit();
        if (eiSlots[index].item == null)
            return;

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
}