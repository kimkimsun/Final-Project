using UnityEngine;

public class ItemSpawnObject : MonoBehaviour
{
    public ItemSpawnList itemSpawnList;
    void Start()
    {
        Item spawnPrefab = itemSpawnList[Random.Range(0, itemSpawnList.Count)];
        Instantiate(spawnPrefab, transform.GetChild(0).position, Quaternion.identity);
    }
}
