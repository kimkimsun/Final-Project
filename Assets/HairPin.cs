using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairPin : MonoBehaviour, IStunable
{

    public void Stun()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<IGetStunable>(out IGetStunable stun))
            stun.GetStun();
    }
}