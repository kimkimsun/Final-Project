using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EscapeItemInventory : Inventory
{
    [SerializeField] private EscapeItemSlot[] escapeSlot;
    public EscapeItemSlot[] EscapeSlot
    { 
        get => escapeSlot; set => escapeSlot = value; 
    }
    public override void AddItem(Item item)
    {
        for (int i = 0; i <= escapeSlot.Length; i++)
        {
            if (escapeSlot[i].item == null)
            {
                escapeSlot[i].SetItem(item);
                break;
            }
        }
    }
}