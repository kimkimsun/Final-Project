using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public event Action OnCountChange;
    public TextMeshProUGUI countText;
    public Item item;
    public Image ItemImage;
    public QuickSlot quickSlot;

    private int countItem;
    public int CountItem
    {
        get { return countItem; }
        set
        {
            countItem = value;
            OnCountChange();


        }
    }

    private void Start()
    {
        OnCountChange += ChangeCount;
        CountItem = 0;
    }
    public void ChangeCount()
    {

        countText.text = CountItem.ToString();
    }

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
