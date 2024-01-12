using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class QSlot : Slot
{

    public event Action OnCountChange;
    public TextMeshProUGUI countText;
    public List<Item> items;

    int curItem;
    int countItem;

    public int CurItem
    {
        get => curItem;
        set
        {
            curItem = value;
            if (curItem <= 0)
            {
                curItem = 0;
            }
        }
    }
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
        items = new List<Item>();
        OnCountChange += ChangeCount;
        CountItem = 0;
        CurItem = 0;
    }
    public void ChangeCount()
    {
        countText.text = CountItem.ToString();
    }

    public override void SlotItemUse()
    {
        if (items.Count != 0)
        {
            items[CurItem].gameObject.SetActive(true);
            items[CurItem].itemStrategy.Use();
            items.RemoveAt(CurItem);
            CurItem--;
            CountItem--;
            if (items.Count == 0)
                SetImage(null);

        }
        else if (items.Count <= 0)
            return;

    }
    public override void SetImage(Item setItem)
    {
        if (items.Count == 0)
            ItemImage.sprite = null;
        else
            ItemImage.sprite = setItem.sprite;
    }

}
