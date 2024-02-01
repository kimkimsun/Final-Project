using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartsInventory : Inventory
{
    [SerializeField] private PartsSlot[] partsSlots;
    public PartsSlot[] PartsSlots
    { 
        get => partsSlots; set => partsSlots = value; 
    }
    public void AddParts(EscapeParts parts)
    {
        for (int i = 0; i <= partsSlots.Length; i++)
        {
            if (partsSlots[i].parts == null)
            {
                partsSlots[i].SetParts(parts);
                break;
            }
        }
    }
}