using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Monster : MonoBehaviour
{
    StateMachine<Monster> sm;
    public bool test;

    private void Start()
    {
        sm = new StateMachine<Monster>();
        sm.owner = this;

        sm.AddState("Idle", new MonsterIdleState());
        sm.AddState("Run", new MonsterRunState());
        sm.SetState("Idle");
    }
    private void Update()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, 5f, 1 << 10);
        Debug.Log(cols[0].name);
        if (cols[0] != null)
            Debug.Log("µé¾î¿È");
        else
            return;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 5f);
    }
}