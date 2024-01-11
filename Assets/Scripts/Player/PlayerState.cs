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
        Debug.Log("기본 상태");
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


public class ExhaustionState : PlayerState //탈진상태
{
    
    public override void Enter()
    {
        Debug.Log("탈진 상태");
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



public class MoribundState : PlayerState // 빈사 상태
{

    public override void Enter()
    {
        Debug.Log("빈사 상태");
        damege = 10;
        minusHpCo = MinusHpCo(damege);
        player.playerMove.MoveSpeed = 1.0f;
        player.playerMove.SprintSpeed = 3.2f;
        player.StartCoroutine(minusHpCo);
        //가쁜 숨 사운드
    }

    public override void Exit()
    {
        //가쁜 숨 사운드 종료
        player.StopCoroutine(minusHpCo);
    }

    public override void Update()
    {
       
    }

}
public class CaughtState: PlayerState //몬스터한테 잡혔을때
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