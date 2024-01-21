using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;

public class HaiKen : Monster
{
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
        maxDistance = 20f;
        agent.speed = 5f;
        lookDetectionRange = 20;
    }
    protected override void Update()
    {
        sm.curState?.Update();
        base.Update();
        if (playerLookCol.Length > 0)
        {
            RaycastHit hit;
            Debug.DrawLine(transform.position, transform.position + (transform.forward * maxDistance), Color.blue);
            if(Physics.BoxCast(transform.position,transform.lossyScale,transform.forward,out hit, transform.rotation,maxDistance))
                isPlayerCheck = CheckInLayerMask(hit.collider.gameObject.layer);
            if (isPlayerCheck && sm.curState is not HaiKenRunState && !isStun)
                sm.SetState("Run");
            else if (!isPlayerCheck && !isStun)
                sm.SetState("Idle");
        }
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
