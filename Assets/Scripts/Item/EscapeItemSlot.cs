using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscapeItemSlot : Slot
{
    public Item item;
    public override void SetItem(Item item)
    {
        this.item = item;
        itemImage.color = new Color(1, 1, 1, 1);
        this.itemImage.sprite = item.sprite;
        item.gameObject.SetActive(false);
    }
}