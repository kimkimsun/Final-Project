using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StunLight : MonoBehaviour,IStunable
{
    SphereCollider stunCollider;
    Light lightStun;
    int maxBright;


    private void Start()
    {
        stunCollider = GetComponent<SphereCollider>();
        lightStun = GetComponent<Light>();
        lightStun.intensity = 0;
        stunCollider.radius = 5;
        maxBright = 30;

    }

    public void Stun()
    {
        stunCollider.enabled = true;
        StartCoroutine(BrightLightCo());
    }

    IEnumerator BrightLightCo()
    {
        lightStun.intensity = maxBright;
        while (lightStun.intensity >= 0)
        {
            lightStun.intensity -= 1 ;
            yield return new WaitForSeconds(0.07f);
        }
        stunCollider.enabled = false;
        yield break;
    }

}
