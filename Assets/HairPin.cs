using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairPin : MonoBehaviour, IStunable
{

    public void AttackEnd()
    {
        Debug.Log("���Դ�?");
        Destroy(transform.root.gameObject);
    }
    public void Stun()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("�����Ĥä��Ѥ������Ĥä��Ť����ä���");
        if(other.TryGetComponent<IGetStunable>(out IGetStunable stun))
        {
            Debug.Log("dgiosjdfgijsidfsjk");
            stun.GetStun();
        }
    }
}