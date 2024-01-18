using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UseItemInventory : Inventory
{
    [SerializeField] private UseItemSlot[] slots = new UseItemSlot[5];
    [SerializeField] private UseItemSlot hairPinSlot;

    public UseItemSlot HairPinSlot
    {
        get => hairPinSlot;
        set => hairPinSlot = value;
    }
    public override void AddItem(Item item)
    {
        if(((UseItem)item).useItem_Type == USEITEM_TYPE.HAIRPIN) 
        {
            hairPinSlot.items.Add(item);
            hairPinSlot.SetItem(item);
            hairPinSlot.CountItem++;
            return;
        }
        else if(((UseItem)item).useItem_Type == USEITEM_TYPE.HAIRPIN && hairPinSlot.items[hairPinSlot.CurItem].itemName == item.itemName)
        {
            hairPinSlot.items.Add(item);
            hairPinSlot.CountItem++;
            hairPinSlot.CurItem++;
            return;
        }
        else
        {

            for (int i = 0; i < slots.Length; i++)
            {
            
                if (slots[i].items.Count != 0 && slots[i].items[slots[i].CurItem].itemName == item.itemName)
                {
                    slots[i].items.Add(item);
                    slots[i].CountItem++;
                    slots[i].CurItem++;
                    return;
                }
                else if (slots[i].items.Count == 0)
                {
                    slots[i].items.Add(item);
                    slots[i].SetItem(item);
                    slots[i].CountItem++;
                    return;
                }
            }
        }
    }
    public void QuickItemUse()
    {
        Image dieCount = UIManager.Instance.escapeCircle;
        if (Input.GetKeyDown(KeyCode.Alpha1))
            slots[0].SlotItemUse();        
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            slots[1].SlotItemUse();
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            slots[2].SlotItemUse();
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            slots[3].SlotItemUse();
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            slots[4].SlotItemUse();
        else if (dieCount.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Q))
        {
            if (dieCount.fillAmount > 0.6f)
                hairPinSlot.SlotItemUse();
            else
                ScenesManager.Instance.DieScene();
        }

    }

    private void Update()
    {
        QuickItemUse();
    }
}
