using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : SingleTon<ItemManager>
{
    public List<Item> itemList;

    public void CreatePrefab(int ID)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if(itemList[i].itemID == ID)
                Instantiate(itemList[i],GameManager.Instance.player.itemBox.transform);
        }
    }
}
