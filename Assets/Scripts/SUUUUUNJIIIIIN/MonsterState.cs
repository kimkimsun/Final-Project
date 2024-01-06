using CustomInterface;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}

public class MonsterRunState : MonsterState
{

    public override void Enter()
    {
        Debug.Log("����");
    }
    public override void Exit()
    {
    }
    public override void Update()
    {
        Debug.Log("��������Ʈ��");
        monster.Agent.SetDestination(monster.PlayerLookCol[0].transform.position);
        // �÷��̾� ������ �����Ÿ��ų�, ��� �������ų� �ؾߵ�
    }
}
public class MonsterStunState : MonsterState
{
    public override void Enter()
    {
        // ++) �ͽ� ��� �Ҹ� �߰�
        monster.StartCoroutine(monster.StunCo());
        monster.Agent.enabled = false;
    }
    public override void Exit()
    {
        monster.Agent.enabled = true;
    }
    public override void Update()
    {
    }
}