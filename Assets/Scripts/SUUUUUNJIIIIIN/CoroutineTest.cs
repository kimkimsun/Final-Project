using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineTest : MonoBehaviour
{
    int a = 5;
    IEnumerator aaa;
    IEnumerator bbb;
    private void Start()
    {
        aaa = aa();
        bbb = bb();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            StopCoroutine(bbb);
            StartCoroutine(aaa);
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            StopCoroutine(aaa);
            StartCoroutine(bbb);
        }
    }
    IEnumerator aa()
    {
        while(a > 0)
        {
            a--;
            yield return new WaitForSeconds(1);
            Debug.Log(a);
            yield return new WaitUntil(() => a > 0);
        }
    }
    IEnumerator bb()
    {
        while (a < 10)
        {
            a++;
            yield return new WaitForSeconds(1);
            Debug.Log(a);
            yield return new WaitUntil(() => a < 10);
        }
    }
}