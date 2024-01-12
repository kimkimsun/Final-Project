using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class InvenSlot : Slot
{

    public Item item;

    public override void SetImage(Item setItem)
    {
        if (item == null)
            ItemImage.sprite = null;
        else
            ItemImage.sprite = setItem.sprite;
    }

    public override void SlotItemUse()
    {
        if (item != null)
        {
            item.gameObject.SetActive(true);
            item.itemStrategy.Use();
        }
        else
            return;
    }


}
