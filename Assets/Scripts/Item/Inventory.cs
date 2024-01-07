using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Slot[] equipSlot = new Slot[5];
    [SerializeField] private Slot tempSlot;
    [SerializeField] private Slot playerEquipSlot;

    private int index;
    public Slot[] EquipSlot
    {
        get { return equipSlot; }
        set { equipSlot = value; }
    }
    public void IndexSlot(int index)
    {
        this.index = index;
        for(int i = 0; i < equipSlot.Length; i++)
        {
            if(i == index)
                equipSlot[i].GetComponent<Image>().color = Color.yellow;
            else
                equipSlot[i].GetComponent<Image>().color = Color.red;
        }
    }
    public void ResetSlot()
    {
        Debug.Log("¸®¼Âµé¾î¿È");
        for (int j = 0; j < equipSlot.Length; j++)
        {
            equipSlot[j].GetComponent<Image>().color = Color.red;
        }
    }
    public void SwitchItem()
    {
        tempSlot.item = equipSlot[index].item;
        tempSlot.GetComponent<Image>().sprite = equipSlot[index].GetComponent<Image>().sprite;
        equipSlot[index].item = playerEquipSlot.item;
        equipSlot[index].GetComponent<Image>().sprite = playerEquipSlot.GetComponent<Image>().sprite;
        playerEquipSlot.item = tempSlot.item;
        playerEquipSlot.GetComponent<Image>().sprite = tempSlot.GetComponent<Image>().sprite;
        tempSlot.item = null;
        tempSlot.GetComponent<Image>().sprite = null;
        ItemBuff();
    }
    public void ItemBuff()
    {
        playerEquipSlot.item.Active();
    }
}
