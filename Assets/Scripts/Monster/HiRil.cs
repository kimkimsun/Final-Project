using CustomInterface;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiRil : Monster
{
    private int soundDetectionRange;
    protected new StateMachine<HiRil> sm;
    protected override void Start()
    {
        sm = new StateMachine<HiRil>();
        sm.owner = this;
        sm.AddState("Idle", new HiRilIdleState());
        sm.AddState("Run", new HiRilRunState());
        sm.AddState("Stun", new HiRilStunState());
        sm.AddState("Attack", new HiRilAttackState());
        sm.SetState("Idle");

        base.Start();
        maxDistance = 20f;
        agent.speed = 5f;
        lookDetectionRange = 20;
        soundDetectionRange = 17;
    }

    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
            sm.SetState("Stun");
        if (Input.GetKeyDown(KeyCode.Keypad1))
            Debug.Log("Test" + sm.curState);
        sm.curState?.Update();
        base.Update();
        soundCol = Physics.OverlapSphere(transform.position, soundDetectionRange, heardTargetLayerMask);
        Debug.Log(playerAttackCol.Length);
        if (soundCol.Length > 0 && !isStun)
        {
            if (soundCol[0].gameObject.layer == 8)
                footTrans = soundCol[0].gameObject.transform;
            soundCol[0] = null;
            sm.SetState("Run");
        }
        else if (playerLookCol.Length > 0 && !isStun)
        {
            Debug.Log("만났다");
            RaycastHit hit;
            Vector3 direction = ((playerLookCol[0].transform.position) - transform.position).normalized;
            Debug.DrawLine(transform.position, transform.position + (direction * maxDistance), Color.blue);
            if (Physics.Raycast(transform.position, direction, out hit, maxDistance))
                isPlayerCheck = CheckInLayerMask(hit.collider.gameObject.layer);
            if (isPlayerCheck && sm.curState is not HiRilRunState && !isStun)
                sm.SetState("Run");
            else if (!isPlayerCheck)
                sm.SetState("Idle");
        }
        else if (sm.curState is not HiRilStunState && sm.curState is not HiRilAttackState)
            sm.SetState("Idle");
        if (playerAttackCol.Length > 0 && !isAttack)
        {
            Debug.Log("공격");
            sm.SetState("Attack");
            isAttack = true;
            isStun = true;
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, soundDetectionRange);
    }
    public override IEnumerator StunCo()
    {
        Debug.Log("스턴 들어옴");
        gameObject.layer = 0;
        isStun = true;
        isAttack = true;
        stunTime = 0f;
        while (stunTime < 5.0f)
        {
            stunTime += Time.deltaTime;
            yield return null;
        }
        sm.SetState("Idle");
        isStun = false;
        isAttack = false;
        gameObject.layer = 9;
    }
    public override void GetStun()
    {
        if(sm.curState is not HiRilStunState sd)
        {
            Debug.Log("한번만 호출");
            sm.SetState("Stun");
        }
    }
}