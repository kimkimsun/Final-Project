using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UseItemInventory : Inventory
{
    [SerializeField] private UseItemSlot[] slots = new UseItemSlot[5];
    [SerializeField] private UseItemSlot hairPinSlot;


    public override void AddItem(Item item)
    {
        if(((UseItem)item).useItem_Type == USEITEM_TYPE.HAIRPIN) 
        {
            hairPinSlot.SetItem(item);
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
        else if(UIManager.Instance.escapeCircle.fillAmount > 0.6f)
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                hairPinSlot.SlotItemUse();
            }
        }
    }

    private void Update()
    {
        QuickItemUse();
    }
}
