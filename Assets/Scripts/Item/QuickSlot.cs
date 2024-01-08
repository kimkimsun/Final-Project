using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class QuickSlot : MonoBehaviour
{
    public Slot[] slots = new Slot[5];

    private void Start()
    {
        for (int i = 0; i < slots.Length; i++) 
        {
            slots[i] =  transform.GetChild(i).GetComponent<Slot>();

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

    }


    public void setItem(Item item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].SetImage(item);

                return;
            }
            if (slots[i].item.itemName == item.itemName)
            {
                slots[i].CountItem++;
            }
        }
    }

    private void Update()
    {
        QuickItemUse();
    }
}
