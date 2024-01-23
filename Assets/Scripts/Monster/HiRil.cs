using CustomInterface;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiRil : Monster
{
    private int soundDetectionRange;
    protected new StateMachine<HiRil> sm;

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
        sm = new StateMachine<HiRil>();
        sm.owner = this;
        sm.AddState("Idle", new HiRilIdleState());
        sm.AddState("Run", new HiRilRunState());
        sm.AddState("Stun", new HiRilStunState());
        sm.AddState("Attack", new HiRilAttackState());
        sm.AddState("Final", new HiRilFinalState());
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
        soundCol = Physics.OverlapSphere(transform.position, soundDetectionRange, heardTargetLayerMask);
        if (soundCol.Length > 0 && !isStun)
        {
            if (soundCol[0].gameObject.layer == 8)
                footTrans = soundCol[0].gameObject.transform;
            soundCol[0] = null;
            sm.SetState("Run");
        }
        else if (playerLookCol.Length > 0 && !isStun)
        {
            RaycastHit hit;
            Vector3 direction = ((playerLookCol[0].transform.position) - transform.position).normalized;
            Debug.DrawLine(transform.position, transform.position + (direction * maxDistance), Color.red);
            if (Physics.Raycast(transform.position, direction+ new Vector3(0,-3,0), out hit, maxDistance))
            {
                Debug.Log(hit.collider.gameObject.name);
                isPlayerCheck = CheckInLayerMask(hit.collider.gameObject.layer);
                Debug.Log("TTTTTTTTT" + isPlayerCheck);
            }
            if (isPlayerCheck && sm.curState is not HiRilRunState && !isStun)
                sm.SetState("Run");
            else if (!isPlayerCheck && !isStun)
                sm.SetState("Idle");
        }
        else if (sm.curState is not HiRilStunState && sm.curState is not HiRilAttackState)
            sm.SetState("Idle");
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, soundDetectionRange);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            if(sm.curState is not HiRilAttackState)
            {
                isStun = true;
                sm.SetState("Attack");
            }
        }
    }
    public override IEnumerator StunCo()
    {
        gameObject.layer = 0;
        isStun = true;
        while (true)
        {
            yield return new WaitForSeconds(7);
            sm.SetState("Idle");
            isStun = false;
            gameObject.layer = 9;
            yield break;
        }
    }
    public override void GetStun()
    {
        if (sm.curState is not HiRilStunState)
            IsStun = true;
    }
}