using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*public static class BrightLight
{
    
    public static void Action(GameObject gameObject)
    {
        
    }

   static IEnumerator BrightLightCo(Light light, int bright, GameObject gameObject)
    {
        light.intensity = bright;
        while (light.intensity > 0)
        {
            light.intensity -= 1;
            yield return new WaitForSeconds(0.07f);

        }
        GameObject.Destroy(gameObject);
        yield break;
    }
}
*/
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
        stunCollider.enabled = true;
        StartCoroutine(BrightLightCo());
    }

    IEnumerator BrightLightCo()
    {
        lightStun.intensity = maxBright;
        while (lightStun.intensity > 0)
        {
            lightStun.intensity -= 1 ;
            yield return new WaitForSeconds(0.07f);

        }
        Destroy(Camera.gameObject);
        yield break;
    }

}
