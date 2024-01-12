using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlot : MonoBehaviour, IStorable
{
    [SerializeField] private QSlot[] slots = new QSlot[5];

    public void AddItem(Item item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            Debug.Log(slots[i] == null);
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
                slots[i].SetImage(item);
                slots[i].CountItem++;
                return;
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
/*        else if (Input.GetKeyDown(KeyCode.F))
            portableSlot[0].Use();*/
    }

    private void Update()
    {
        QuickItemUse();
    }
}
