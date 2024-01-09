using UnityEngine;

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
            if ( slots[i].items.Count != 0 && slots[i].items[slots[i].CurItem].itemName== item.itemName)
            {
                Debug.Log("들어왔던 아이템");
                slots[i].items.Add(item);
                slots[i].CountItem++;
                slots[i].CurItem++;
                return;
            }
            else if (slots[i].items.Count == 0)
            {
                Debug.Log("처음들어온 아이템");
                slots[i].items.Add(item);
                slots[i].SetImage(item);
                slots[i].CountItem++;
                return;
            }
        }
    }
    private void Update()
    {
        QuickItemUse();
    }
}
