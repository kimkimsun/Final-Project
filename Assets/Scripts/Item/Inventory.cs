using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]private Slot[] equipSlot = new Slot[5];

    public Slot[] EquipSlot
    {
        get { return equipSlot; }
        set { equipSlot = value; }
    }
    public void IndexSlot(int index)
    {
        for(int i = 0; i < equipSlot.Length; i++)
        {
            if(i == index)
                equipSlot[i].GetComponent<Image>().color = Color.yellow;
            else
                equipSlot[i].GetComponent<Image>().color = Color.red;
        }
    }
}
