using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item item;
    public event Action OnCountChange;
    public TextMeshProUGUI countText;
    public List <Item> items;
    public Image ItemImage;
    public QuickSlot quickSlot;

    int curItem; 
    int countItem;
    public int CurItem
    {
        get => curItem;
        set{curItem = value;}
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

    public void SlotItemUse()
    {
        if (items != null)
        {
            items[CurItem].gameObject.SetActive(true);
            items[CurItem].itemStrategy.Use();
            items.RemoveAt(CurItem);
            CurItem--;
            CountItem--;
            if (items.Count == 0)
                SetImage(null);

        }
        else if (items.Count == 0)
            return;
        
    }
    public void SetImage(Item setItem)
    {
        if (items.Count == 0)
            ItemImage.sprite = null;
        else
            ItemImage.sprite = setItem.sprite;
    }
}