using UnityEngine;

public class ItemSpawnObject : MonoBehaviour
{
    public ItemSpawnList itemSpawnList;
    void Start()
    {
        int randomSpawn = Random.Range(0, 2);

        if(randomSpawn == 0)
        {
            Item spawnPrefab = itemSpawnList[Random.Range(0, itemSpawnList.Count)];
            Instantiate(spawnPrefab, transform.GetChild(0).position, Quaternion.identity);
        }
    }
}
