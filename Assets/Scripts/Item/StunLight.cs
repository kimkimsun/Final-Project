using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StunLight : MonoBehaviour,IStunable
{
    GameObject Camera;
    SphereCollider stunCollider;
    Light lightStun;
    
    int maxBright;


    private void Start()
    {
        Camera = transform.parent.gameObject;
        stunCollider = GetComponent<SphereCollider>();
        lightStun = GetComponent<Light>();
        lightStun.intensity = 0;
        stunCollider.radius = 5;
        maxBright = 30;

    }

    public void Stun()
    {
        StartCoroutine(BrightLightCo());
    }

    IEnumerator BrightLightCo()
    {
        lightStun.intensity = maxBright;
        while (lightStun.intensity > 0)
        {
            stunCollider.enabled = true;
            lightStun.intensity -= 1 ;
            yield return new WaitForSeconds(0.07f);

        }
        Destroy(Camera.gameObject);
        yield break;
    }

}
