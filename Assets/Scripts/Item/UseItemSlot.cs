using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class UseItemSlot : Slot
{

    public List<Item> items;
    public TextMeshProUGUI countText;
    public event Action OnUse;
    private event Action OnCountChange;

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
        if (items.Count <= 0)
        {
            return;
        }
        else if (items.Count != 0)
        {
            items[CurItem].gameObject.SetActive(true);
            items[CurItem].itemStrategy.Use();
            items.RemoveAt(CurItem);
            CurItem--;
            CountItem--;
            if (items.Count == 0)
                SetItem(null);
            OnUse?.Invoke();
        }
    }
    public override void SetItem(Item setItem)
    {
        if (items.Count == 0)
            itemImage.sprite = null;
        else
        {
            itemImage.color = new Color(1, 1, 1, 1);
            countText.color = new Color(0, 0, 0, 1);
            itemImage.sprite = setItem.sprite;
        }

    }

}
