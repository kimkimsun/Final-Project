using CustomInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ActionNode : INode
{
    public Func<INode.STATE> func;
    public ActionNode(Func<INode.STATE> func)
    {
        this.func += func;
    }
    public INode.STATE Evaluate()
    {
        if (func == null)
            return INode.STATE.FAIL;
        else
            return func();
    }
}
public class SelectorNode : INode
{
    public Func<INode.STATE> func;
    List<INode> children;

    public SelectorNode(Func<INode.STATE> func)
    {
        children = new List<INode>();
        this.func += func;
    }
    public void Add(INode node)
    {
        children.Add(node);
    }

    public INode.STATE Evaluate()
    {
        foreach (INode child in children)
        {
            INode.STATE iState = child.Evaluate();
            switch (iState)
            {
                case INode.STATE.RUN:
                    return INode.STATE.RUN;
                case INode.STATE.SUCCESS:
                    return INode.STATE.SUCCESS;
            }
        }
        return INode.STATE.FAIL;
    }
}
public class SequenceNode : INode
{
    public Func<INode.STATE> func;
    List<INode> children;
    public SequenceNode(Func<INode.STATE> func)
    {
        children = new List<INode>();
        this.func += func;
    }
    public void Add(INode node)
    {
        children.Add(node);
    }
    public INode.STATE Evaluate()
    {
        foreach (INode child in children)
        {
            switch (child.Evaluate())
            {
                case INode.STATE.RUN:
                    return INode.STATE.RUN;
                case INode.STATE.SUCCESS:
                    continue;
                case INode.STATE.FAIL:
                    return INode.STATE.FAIL;
            }
        }
        return INode.STATE.SUCCESS;
    }
}
public abstract class MonsterState : State
{
    protected Monster monster;
    protected IEnumerator monsterMoveCo;
    public override void Init(IStateMachine sm)
    {
        this.sm = sm;
        monster = (Monster)sm.GetOwner();
        monsterMoveCo = monster.MonsterMoveCo();
    }
    public override void Enter() { }
    public override void Update() { }
    public override void Exit() { }
}

public class MonsterIdleState : MonsterState
{
    public override void Enter()
    {
        monster.StartCoroutine(monsterMoveCo);
    }
    public override void Exit()
    {
        monster.StopCoroutine(monsterMoveCo);
    }
    public override void Update()
    {
        Debug.Log("IDLE 들어옴");
    }
}

public class MonsterRunState : MonsterState
{
    SelectorNode rootNode;
    SequenceNode playerSequence;
    SequenceNode firecrackerSequence;
    ActionNode fireCrackerAction;
    ActionNode playerAction;
    private void Start()
    {
        rootNode.Add(playerSequence = new SequenceNode());
        rootNode.Add(firecrackerSequence = new SequenceNode());

        playerSequence.Add(playerAction = new Action());
        firecrackerSequence.Add(fireCrackerAction = new Action());
    }
    public override void Enter()
    { 
        monster.Animator.SetBool("isRun", true);
        monster.Agent.speed = 7;
    }
    public override void Exit()
    {
        monster.Animator.SetBool("isRun", false);
    }
    public override void Update()
    {
        Debug.Log("런스테이트임");
        monster.Agent.SetDestination(monster.PlayerLookCol[0].transform.position);
        // 플레이어 손전등 깜빡거리거나, 밝기 낮아지거나 해야됨
    }
}
public class MonsterStunState : MonsterState
{
    public override void Enter()
    {
        // ++) 귀신 비명 소리 추가
        monster.StartCoroutine(monster.StunCo());
        monster.Agent.enabled = false;
        monster.Animator.SetBool("isStun", true);
        monster.Animator.bodyRotation = GameManager.Instance.transform.rotation;
    }
    public override void Exit()
    {
        monster.Animator.SetBool("isStun", false);
        monster.Agent.enabled = true;
    }
    public override void Update()
    {
    }
}
public class MonsterAttackState : MonsterState
{
    public override void Enter()
    {
        monster.Agent.enabled = false;
        monster.Animator.SetBool("isAttack",true);
        monster.Escape = 0f;
    }
    public override void Exit()
    {
        monster.Agent.enabled = true;
        monster.Animator.SetBool("isAttack",false);
        monster.StopCoroutine(monster.EscapeCor);
    }
    public override void Update()
    {
    }
}