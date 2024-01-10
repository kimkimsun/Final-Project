using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPrevPos : MonoBehaviour
{
    FirstPersonController playerPos;

    public GameObject prevPos;
    public Queue<GameObject> prevPosQ = new Queue<GameObject>();
    public IEnumerator posSaveCo;
    private void Start()
    {
        playerPos=GetComponent<FirstPersonController>();
        posSaveCo = PosSaveCo();
        playerPos = GetComponent<FirstPersonController>();
        for(int i = 0; i < 100; i++)
        {
            GameObject copyPos = Instantiate(prevPos, Vector3.zero, Quaternion.identity);
            prevPosQ.Enqueue(copyPos);
            copyPos.SetActive(false);

        }
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(posSaveCo);

        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {

            StopCoroutine(posSaveCo);
        }
    }

    IEnumerator PosSaveCo()
    {
        while(playerPos.isMove == true)
        {
            GameObject pos = GetQueue(this.transform);
            yield return new WaitForSeconds(0.5f);
            pos.transform.position = Vector3.zero;
        }
    }
    public void PushObject(GameObject obj)
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
        prevPosPrefab.SetActive(false);
    }
}