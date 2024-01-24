using CustomInterface;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
#region BT
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
#endregion
public class HiRilState : MonsterState
{
    //protected HiRil owner;
    //public HiRilState()
    //{
    //    owner = (HiRil)sm.GetOwner();
    //    monsterMoveCo = owner.MonsterMoveCo();
    //}
    public override void Init(IStateMachine sm)
    {
        this.sm = sm;
        owner = (HiRil)sm.GetOwner();
        monsterMoveCo = owner.MonsterMoveCo();
    }
    public override void Enter() { }
    public override void Update() { }
    public override void Exit() { }
}
public class HiRilIdleState : HiRilState
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
public class HiRilRunState : HiRilState
{
    SelectorNode rootNode;
    SequenceNode heardSequence;
    SequenceNode detectiveSequence;
    ActionNode fireCrackerChaseAction;
    ActionNode footChaseAction;
    ActionNode playerChaseAction;

    public HiRilRunState()
    {
        FirstSetting();
    }
    public void FirstSetting()
    {
        rootNode = new SelectorNode(() => { return INode.STATE.SUCCESS; });
        rootNode.Add(detectiveSequence = new SequenceNode(() =>
        {
            if (owner.IsPlayerCheck)
                return INode.STATE.SUCCESS;
            else
                return INode.STATE.FAIL;
        }));

        rootNode.Add(heardSequence = new SequenceNode(() =>
        {
            if (owner.IsHeardCheck)
                return INode.STATE.SUCCESS;
            else
                return INode.STATE.FAIL;
        }));

        detectiveSequence.Add(playerChaseAction = new ActionNode(() =>
        {
            if (owner.PlayerLookCol.Length > 0)
            {
                owner.Agent.SetDestination(owner.PlayerLookCol[0].transform.position);
                return INode.STATE.RUN;
            }
            else
                return INode.STATE.FAIL;
        }));

        heardSequence.Add(footChaseAction = new ActionNode(() =>
        {
            if (owner.FootTrans != null)
            {
                while (Vector3.Distance(owner.transform.position, owner.FootTrans.position) > 1.5f)
                {
                    owner.Agent.SetDestination(owner.FootTrans.transform.position);
                    return INode.STATE.RUN;
                }
                sm.SetState("Idle");
                return INode.STATE.FAIL;
            }
            else
                return INode.STATE.FAIL;
        }));
        heardSequence.Add(fireCrackerChaseAction = new ActionNode(() =>
        {
            if (owner.SoundCol.Length > 0)
            {
                while (Vector3.Distance(owner.transform.position, owner.FootTrans.position) > 1.5f)
                {
                    owner.Agent.SetDestination(owner.SoundCol[0].transform.position);
                    Debug.Log("ÆøÁ× µé¾î¿È");
                    return INode.STATE.RUN;
                }
                sm.SetState("Idle");
                return INode.STATE.FAIL;
            }
            else
                return INode.STATE.FAIL;
        }));
    }
    public override void Enter()
    {
        owner.Animator.SetBool("isRun", true);
    }
    public override void Exit()
    {
        owner.Animator.SetBool("isRun", false);
    }
    public override void Update()
    {
        rootNode.Evaluate();
        // ÇÃ·¹ÀÌ¾î ¼ÕÀüµî ±ôºý°Å¸®°Å³ª, ¹à±â ³·¾ÆÁö°Å³ª ÇØ¾ßµÊ
    }
}
public class HiRilStunState : HiRilState
{
    public override void Enter()
    {
        owner.gameObject.layer = 0;
        owner.Agent.Move(owner.transform.forward * -35 * Time.deltaTime);
        owner.Agent.isStopped = true;
        owner.StartCoroutine(((HiRil)owner).StunCo());
        owner.Animator.SetBool("isStun", true);
    }
    public override void Exit()
    {
        owner.Animator.SetBool("isStun", false);
        owner.Agent.isStopped = false;
    }
    public override void Update()
    {
    }
}
public class HiRilAttackState : HiRilState
{
    public override void Enter()
    {
        owner.Agent.isStopped = true;
        owner.Animator.SetBool("isAttack", true);
        owner.Escape = 0f;
    }
    public override void Exit()
    {
        owner.Agent.isStopped = false;
        owner.Animator.SetBool("isAttack", false);
    }
    public override void Update() { }
}
public class HiRilFinalState : HiRilState
{
    public override void Enter()
    {
        owner.Animator.SetBool("isRun", true);
    }
    public override void Exit()
    {
    }
    public override void Update() 
    {
        owner.Agent.SetDestination(GameManager.Instance.player.transform.position);
    }
}
