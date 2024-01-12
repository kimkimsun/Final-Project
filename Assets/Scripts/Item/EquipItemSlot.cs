using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipItemSlot : Slot
{
    public Item item;
    public override void SetItem(Item item)
    {
        this.item = item;
        itemImage.sprite = item.sprite;
        item.gameObject.SetActive(false);
    }
}