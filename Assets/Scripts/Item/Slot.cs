using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Slot : MonoBehaviour
{
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
        items = new List <Item> ();
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
            items[CurItem].itemStrategy.Use();
            CurItem--;
            items.RemoveAt(CurItem);
            SetImage(null);

        }
    }
    public void SetImage(Item setItem)
    {
        items[curItem] = setItem;
        if (items == null)
            ItemImage.sprite = null;
        else
            ItemImage.sprite = items[curItem].sprite;
    }

}
