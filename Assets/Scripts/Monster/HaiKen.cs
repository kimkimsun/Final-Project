using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class HaiKen : Monster
{
    protected new StateMachine<HaiKen> sm;
    
    protected override void Start()
    {
        sm = new StateMachine<HaiKen>();
        sm.owner = this;
        sm.AddState("Idle", new HaiKenIdleState());
        sm.AddState("Run", new HaiKenRunState());
        sm.AddState("Stun", new HaiKenStunState());
        sm.AddState("Attack", new HaiKenAttackState());
        base.Start();

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
            if (Physics.Raycast(transform.position, new Vector3(transform.position.x,transform.position.y * 3), out hit, maxDistance))
                isPlayerCheck = CheckInLayerMask(hit.collider.gameObject.layer);
            if (isPlayerCheck && sm.curState is HaiKenIdleState hi && isStun)
                sm.SetState("Run");
            else if (!isPlayerCheck)
                sm.SetState("Idle");
            else
                return;
        }
        else
            sm.SetState("Idle");
        if (playerAttackCol.Length > 0 && isAttack)
        {
            sm.SetState("Attack");
            isAttack = false;
            isStun = false;
        }
    }
    public void EndAnimation()
    {
        agent.isStopped = false;
    }
}
