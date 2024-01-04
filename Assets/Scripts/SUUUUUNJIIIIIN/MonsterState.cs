using CustomInterface;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterState : State
{
    protected Monster monster;
    public override void Init(IStateMachine sm)
    {
        this.sm = sm;
        monster = (Monster)sm.GetOwner();
    }
    public override void Enter() { }
    public override void Update() { }
    public override void Exit() { }
}

public class MonsterIdleState : MonsterState
{

    public override void Enter()
    {
        monster.StartCoroutine(monster.MonsterMoveCo());
    }
    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}

public class MonsterRunState : MonsterState
{

    public override void Enter()
    {

    }
    public override void Exit()
    {
    }
    public override void Update()
    {
    }
}