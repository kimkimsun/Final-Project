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
        sm.curState?.Update();
        base.Update();
        soundCol = Physics.OverlapSphere(transform.position, soundDetectionRange, heardTargetLayerMask); // 둘 다 base.Update()하고 여기부터는 따로 정의 해야할 듯?
        if (soundCol.Length > 0 && isStun) // 히리르만 사용
        {
            sm.SetState("Run");
            if (soundCol[0].gameObject.layer == 8)
                footTrans = soundCol[0].gameObject.transform;
            soundCol[0] = null;
            sm.SetState("Run");
        } // 히리르만 사용
        else if (playerLookCol.Length > 0 && isStun)
        {
            RaycastHit hit;
            Vector3 direction = ((playerLookCol[0].transform.position) - transform.position).normalized;
            Debug.DrawLine(transform.position, transform.position + (direction * maxDistance), Color.blue);
            if (Physics.Raycast(transform.position, direction, out hit, maxDistance))
                isPlayerCheck = CheckInLayerMask(hit.collider.gameObject.layer);
            if (isPlayerCheck && sm.curState is not HiRilRunState && isStun)
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

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, soundDetectionRange);
    }
    public override IEnumerator StunCo()
    {
        gameObject.layer = 0;
        isStun = false;
        isAttack = false;
        stunTime = 0f;
        while (stunTime < 5.0f)
        {
            stunTime += Time.deltaTime;
            yield return null;
        }
        sm.SetState("Idle");
        isStun = true;
        isAttack = true;
        gameObject.layer = 9;
    }
    public override void GetStun()
    {
        if(sm.curState is not HiRilStunState sd)
        {
            Debug.Log("제발 되주세요");
            sm.SetState("Stun");
        }
    }
}