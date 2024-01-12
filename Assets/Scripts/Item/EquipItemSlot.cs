using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipItemSlot : Slot
{
    private Item item;
    private Sprite itemSprite;
    public Image ItemImage
    {
        get => itemImage;   set => itemImage = value;
    }
    public Item Item
    {
        get => item;    set => item = value;    
    }
    public Sprite ItemSprite
    {
        get => itemSprite; set => itemSprite = value;
    }
    private void Start()
    {
        itemImage = GetComponent<Image>();
        itemSprite = itemImage.sprite;
    }
    public override void SetItem(Item item)
    {
        this.item = item;
        itemSprite = item.sprite;
        item.gameObject.SetActive(false);
    }
}