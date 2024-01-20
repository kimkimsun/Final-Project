using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : SingleTon<ItemManager>
{
    public List<Item> itemList;

    public Item CreatePrefab(int ID)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].itemID == ID)
                return Instantiate(itemList[i]);
        }
        return null;
    }
}
