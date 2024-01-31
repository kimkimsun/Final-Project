using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipItemInventory : Inventory
{
    [SerializeField] private EquipItemSlot[] eiSlots;
    [SerializeField] private EquipItemSlot flashLightPocket;
    public EquipItemSlot[] EiSlots
    { 
        get => eiSlots; set => eiSlots = value; 
    }
    public override void AddItem(Item item)
    {
        for (int i = 0; i <= eiSlots.Length; i++)
        {
            if (eiSlots[i].item == null)
            {
                eiSlots[i].SetItem(item);
                break;
            }
        }
    }
}