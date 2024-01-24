using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairPin : MonoBehaviour, IStunable
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<IGetStunable>(out IGetStunable stun))
            Stun(stun);
    }
    public void Stun(IGetStunable target)
    {
        Debug.Log("몬스터 스턴좀 먹여라 제발");
        target.GetStun();
        Destroy(transform.parent.gameObject);
    }
}