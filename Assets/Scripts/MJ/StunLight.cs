using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StunLight : MonoBehaviour,IStunable
{
    Light lightStun;
    int maxBright;


    private void Start()
    {
        lightStun = GetComponent<Light>();
        lightStun.intensity = 0;
        maxBright = 30;

    }

    public void Stun()
    {
        StartCoroutine(BrightLightCo());
    }

    IEnumerator BrightLightCo()
    {
       
        while (lightStun.intensity <= maxBright)
        {
            lightStun.intensity += 1 ;
            yield return new WaitForSeconds(0.07f);
        }
        lightStun.intensity = 0;
        yield break;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.N)) 
        {
            Stun();
        }
    }
}
