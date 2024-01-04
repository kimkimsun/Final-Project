using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : State
{
    public MainPlayer player;

    protected int damege;
    protected float plus;
    protected float time;
    protected float stamina;
    protected IEnumerator plusStaminaCo;
    protected IEnumerator minusHpCo;
    protected PlayerState()
    {
        player = (MainPlayer)sm.GetOwner();
        plusStaminaCo = PlusStaminaCo(plus, time);
        minusHpCo = MinusHpCo(damege);
    }
    protected IEnumerator PlusStaminaCo(float plus, float time)
    {
        while (stamina > 100)
        {
            yield return new WaitForSeconds(time);
            stamina += plus;

        }      
    }

    protected IEnumerator MinusHpCo(int damege)
    {
        while (player.Hp > 0)
        {
            yield return new WaitForSeconds(3);
            player.Hp -= damege;
        }
        yield break;
    }
}

public class IdleState : PlayerState
{
    public override void Enter()
    {
        plus = 5f;
        time = 2f;
        player.StartCoroutine(PlusStaminaCo(plus, time));
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        
    }
}


public class ExhaustionState : PlayerState //Å»Áø»óÅÂ
{

    public override void Enter()
    {
        plus = 2.5f;
        time = 2f;
        damege = 5;
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



public class MoribundState : PlayerState // ºó»ç »óÅÂ
{

    public override void Enter()
    {
        plus = 2.5f;
        time = 3f;
        damege = 10;
        player.playerMove.MoveSpeed = 2.0f;
        player.playerMove.SprintSpeed = 3.2f;       
        //°¡»Û ¼û »ç¿îµå
    }

    public override void Exit()
    {
        //°¡»Û ¼û »ç¿îµå Á¾·á
        player.StopCoroutine(minusHpCo);
    }

    public override void Update()
    {
       
    }

}


