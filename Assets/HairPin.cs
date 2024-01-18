using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairPin : MonoBehaviour, IStunable
{

    public void AttackEnd()
    {
        Debug.Log("들어왔니?");
        Destroy(transform.root.gameObject);
    }
    public void Stun()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ㅁㅇ냐ㅓㅏㅡㅐㅇㄹ냐ㅓㅐㅕㅇ랴ㅓㅐㄴ");
        if(other.TryGetComponent<IGetStunable>(out IGetStunable stun))
        {
            Debug.Log("dgiosjdfgijsidfsjk");
            stun.GetStun();
        }
    }
}