using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaiKenState : MonsterState
{
    public override void Init(IStateMachine sm)
    {
        this.sm = sm;
        this.owner = (HaiKen)sm.GetOwner();
        monsterMoveCo = owner.MonsterMoveCo();
    }
    public override void Enter() { }
    public override void Update() { }
    public override void Exit() { }
}
public class HaiKenIdleState : HaiKenState
{
    public override void Enter()
    {
        owner.StartCoroutine(monsterMoveCo);
    }
    public override void Exit()
    {
        owner.StopCoroutine(monsterMoveCo);
    }
    public override void Update() { }
}
public class HaiKenRunState : HaiKenState
{
    public override void Enter()
    {
        owner.Animator.SetBool("isRun", true);
        owner.Agent.speed = 7;
    }
    public override void Exit()
    {
        owner.Animator.SetBool("isRun", false);
    }
    public override void Update()
    {
        owner.Agent.SetDestination(owner.PlayerLookCol[0].transform.position);
        // ÇÃ·¹ÀÌ¾î ¼ÕÀüµî ±ôºý°Å¸®°Å³ª, ¹à±â ³·¾ÆÁö°Å³ª ÇØ¾ßµÊ
    }
}
public class HaiKenStunState : HaiKenState
{
    public override void Enter()
    {
        // ++) ±Í½Å ºñ¸í ¼Ò¸® Ãß°¡
        owner.StartCoroutine(owner.StunCo());
        owner.Agent.enabled = false;
        owner.Animator.SetBool("isStun", true);
        owner.MonsterVirtualCamera.Priority = 9;
        owner.Animator.bodyRotation = GameManager.Instance.transform.rotation;
    }
    public override void Exit()
    {
        owner.Animator.SetBool("isStun", false);
        owner.Agent.enabled = true;
    }
    public override void Update() { }
}
public class HaiKenAttackState : HaiKenState
{
    public override void Enter()
    {
        owner.Agent.enabled = false;
        owner.Animator.SetBool("isAttack", true);
        owner.MonsterVirtualCamera.Priority = 11;
        owner.Escape = 0f;
    }
    public override void Exit()
    {
        owner.Agent.enabled = true;
        owner.Animator.SetBool("isAttack", false);
        owner.StopCoroutine(owner.EscapeCor);
    }
    public override void Update() { }
}