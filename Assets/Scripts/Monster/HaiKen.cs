using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;



public class HaiKen : Monster
{
    private float angleRange = 90f;
    protected new StateMachine<HaiKen> sm;

    public bool IsStun
    {
        get => isStun;
        set
        {
            isStun = value;
            if (isStun)
                sm.SetState("Stun");
        }
    }
    protected override void Start()
    {
        finalEvent.RegisterListener(() => { sm.SetState("Final"); });
        sm = new StateMachine<HaiKen>();
        sm.owner = this;
        sm.AddState("Idle", new HaiKenIdleState());
        sm.AddState("Run", new HaiKenRunState());
        sm.AddState("Stun", new HaiKenStunState());
        sm.AddState("Attack", new HaiKenAttackState());
        sm.AddState("Final", new HaiKenFinalState());
        base.Start();
        animator.SetBool("isStart", true);
        agent.isStopped = true;
        maxDistance = 10f;
        agent.speed = 5f;
        lookDetectionRange = 20;
    }
    protected override void Update()
    {
        sm.curState?.Update();
        base.Update();

        RaycastHit hit;
        Vector3 interV = playerLookCol[0].transform.position - transform.position;

        if(interV.magnitude <= maxDistance)
        {
            float dot = Vector3.Dot(interV.normalized, transform.forward);
            float theta = Mathf.Acos(dot);
            float degree = Mathf.Rad2Deg * theta;
            if (Physics.Raycast(transform.position, interV * maxDistance, out hit, maxDistance) && degree <= angleRange / 2f)
                isPlayerCheck = CheckInLayerMask(hit.collider.gameObject.layer);
            if (isPlayerCheck && sm.curState is not HaiKenRunState && !isStun)
                sm.SetState("Run");
        }

        if (!isPlayerCheck && !isStun)
            sm.SetState("Idle");
    
        else if(playerLookCol.Length == 0 && !isStun)
            sm.SetState("Idle");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            if (sm.curState is not HaiKenAttackState)
            {
                isStun = true;
                sm.SetState("Attack");
            }
        }
    }
    protected override void OnDrawGizmos()
    {
        Handles.color = isPlayerCheck ? Color.red : Color.blue;
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, angleRange / 2, maxDistance);
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -angleRange / 2, maxDistance);
    }
    public void EndAnimation()
    {
        Animator.SetBool("isStart", false);
        agent.isStopped = false;
    }
    public override IEnumerator StunCo()
    {
        //trigger충돌나게 해야됨
        isStun = true;
        while (true)
        {
            yield return new WaitForSeconds(7);
            sm.SetState("Idle");
            isStun = false;
        }
    }
    public override void GetStun()
    {
        if (sm.curState is not HaiKenStunState)
            IsStun = true;
    }
}