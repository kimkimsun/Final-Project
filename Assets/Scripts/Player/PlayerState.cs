using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : State
{
    public Player player;

    protected int damege;

    protected IEnumerator minusHpCo;

    public override void Init(IStateMachine sm)
    {
        this.sm = sm;
        player = (Player)sm.GetOwner();
    }

    protected IEnumerator MinusHpCo(int damege)
    {
        while (player.Hp > 0)
        {
            yield return new WaitForSeconds(3);
            player.Hp -= damege;
            Debug.Log(damege);
        }
        yield break;
    }
}

public class IdleState : PlayerState
{
    public override void Enter()
    {
        Debug.Log("�⺻ ����");
        player.playerMove.MoveSpeed = 4.0f;
        player.playerMove.SprintSpeed = 6.0f;
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        
    }
}


public class ExhaustionState : PlayerState //Ż������
{
    
    public override void Enter()
    {
        Debug.Log("Ż�� ����");
        damege = 5;
        minusHpCo = MinusHpCo(damege);
        player.playerMove.MoveSpeed = 3.0f;
        player.playerMove.SprintSpeed = 4.5f;
        player.StartCoroutine(minusHpCo);

    }

    public override void Exit()
    {
        player.StopCoroutine(minusHpCo);
    }

    public override void Update()
    {
       
    }
}



public class MoribundState : PlayerState // ��� ����
{

    public override void Enter()
    {
        Debug.Log("��� ����");
        damege = 10;
        minusHpCo = MinusHpCo(damege);
        player.playerMove.MoveSpeed = 1.0f;
        player.playerMove.SprintSpeed = 3.2f;
        player.StartCoroutine(minusHpCo);
        //���� �� ����
    }

    public override void Exit()
    {
        //���� �� ���� ����
        player.StopCoroutine(minusHpCo);
    }

    public override void Update()
    {
       
    }

}
public class CaughtState: PlayerState //�������� ��������
{

    public override void Enter()
    {
        player.playerMove.enabled = false;
        
    }

    public override void Exit()
    {
        player.playerMove.enabled = true;
    }

    public override void Update()
    {

    }
}