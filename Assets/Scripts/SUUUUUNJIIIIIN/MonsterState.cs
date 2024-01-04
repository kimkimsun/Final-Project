using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterState : State
{
}

public class MonsterIdleState : MonsterState
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

public class MonsterRunState : MonsterState
{

    public override void Enter()
    {
        Monster monster = (Monster)sm.GetOwner();
    }
    public override void Exit()
    {
    }
    public override void Update()
    {
    }
}