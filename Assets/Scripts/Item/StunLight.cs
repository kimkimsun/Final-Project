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
        maxBright = 30;

    }

    public void Stun()
    {
        
    }

    IEnumerator BrightLightCo()
    {
        lightStun.intensity = 0;

        while (lightStun.intensity <= 30)
        {
            lightStun.intensity += 1 ;
            yield return new WaitForSeconds(0.5f);
        }

    }
}
