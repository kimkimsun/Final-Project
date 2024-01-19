using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemSpawnList : ScriptableObject
{
    public List<Item> spawnList = new List<Item>();

    public int Count => spawnList.Count;
    
    public Item this[int index]
    {
        get
        {
            return spawnList[index];
        }
    }
}