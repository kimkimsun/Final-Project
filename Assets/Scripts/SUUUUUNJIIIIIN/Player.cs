using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool isActive;

    public int battery;
    public Item flash;

    public bool IsActive
    {
        get => isActive;
        set 
        { 
            isActive = value;
            if(isActive)
            {
                SwitchOn();
            }
            if(!isActive)
            {
                SwitchOff();
            }
        }
    }
    public void OperationFlashLight()
    {
        IsActive = !IsActive;
    }
    private void SwitchOn()
    {
        StartCoroutine("FlashOutCo");
        StopCoroutine("FlashInCo");
    }
    private void SwitchOff()
    {
        StopCoroutine("FlashOutCo");
        StartCoroutine("FlashInCo");
    }
/*    IEnumerator FlashOutCo()
    {
        while(flash.batteryCharge > 0)
        {
            flash.batteryCharge -= Time.deltaTime * a;
             yield return null;
        }
        IsActive = false;
    }
    IEnumerator FlashInCo()
    {
        while (flash.batteryCharge <= 20)
        {
            flash.batteryCharge += Time.deltaTime * b;
            yield return null;
        }
    }*/
}