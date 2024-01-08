using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item item;
    public Image ItemImage;
    public QuickSlot quickSlot;
    public void SlotItemUse()
    {
        if (item != null)
        {

            item.itemStrategy.Use();
            SetImage(null);

        }
    }
    public void SetImage(Item setItem)
    {
        item = setItem;
        if (item == null)
            ItemImage.sprite = null;
        else
            ItemImage.sprite = item.sprite;
    }

}
