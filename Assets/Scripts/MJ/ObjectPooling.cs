using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling instance;
    
    public Queue<GameObject> prevPosQ =  new Queue<GameObject>();
    void Start()
    {
        instance = this;

    }

/*    public void PushObject(GameObject obj)
    {
        for (int i = 0; i < 100; i++)
        {
            GameObject copyPos = Instantiate(obj, Vector3.zero, Quaternion.identity);
            prevPosQ.Enqueue(copyPos);
            copyPos.SetActive(false);

        }
    }
    public GameObject GetQueue(Transform pos)
    {
        GameObject copyObj = prevPosQ.Dequeue();
        copyObj.transform.position = pos.position; 
        copyObj.SetActive(true);
        return copyObj;
    }

    public void InsertQueue(GameObject prevPosPrefab)
    {
        prevPosQ.Enqueue(prevPosPrefab);
        prevPosPrefab.SetActive(false );
    }*/
}
