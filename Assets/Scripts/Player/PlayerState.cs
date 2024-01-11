using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : State
{
    public Player player;

    protected int damege;
    protected float plus;
    protected float time;
    protected IEnumerator plusStaminaCo;
    protected IEnumerator minusHpCo;

    public override void Init(IStateMachine sm)
    {
        this.sm = sm;
        player = (Player)sm.GetOwner();
    }
/*    protected IEnumerator PlusStaminaCo(float plus, float time)
    {
        while (player.Stamina > 100)
        {
            yield return new WaitForSeconds(time);
            player.Stamina += plus * Time.deltaTime;

        }      
    }*/

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
        Debug.Log("±âº» »óÅÂ");
        plus = 5f;
        time = 2f;
        //plusStaminaCo = PlusStaminaCo(plus, time);
        player.playerMove.MoveSpeed = 4.0f;
       // player.StartCoroutine(plusStaminaCo);
    }

    public override void Exit()
    {
        //player.StopCoroutine(plusStaminaCo);
    }

    public override void Update()
    {
        
    }
}


public class ExhaustionState : PlayerState //Å»Áø»óÅÂ
{
    
    public override void Enter()
    {
        Debug.Log("Å»Áø »óÅÂ");
        plus = 2.5f;
        time = 2f;
        damege = 5;
        //plusStaminaCo = PlusStaminaCo(plus, time);
        minusHpCo = MinusHpCo(damege);
        player.playerMove.MoveSpeed = 3.0f;
        player.playerMove.SprintSpeed = 4.5f;
        player.StartCoroutine(minusHpCo);
       // player.StartCoroutine(plusStaminaCo);

    }

    public override void Exit()
    {
        player.StopCoroutine(minusHpCo);
       // player.StopCoroutine(plusStaminaCo);
    }

    public override void Update()
    {
       
    }
}



public class MoribundState : PlayerState // ºó»ç »óÅÂ
{

    public override void Enter()
    {
        Debug.Log("ºó»ç »óÅÂ");
        plus = 2.5f;
        time = 3f;
        damege = 10;
        minusHpCo = MinusHpCo(damege);
        player.playerMove.MoveSpeed = 1.0f;
        player.playerMove.SprintSpeed = 3.2f;
        player.StartCoroutine(minusHpCo);
        //player.StartCoroutine(plusStaminaCo);
        //°¡»Û ¼û »ç¿îµå
    }

    public override void Exit()
    {
        //°¡»Û ¼û »ç¿îµå Á¾·á
        player.StopCoroutine(minusHpCo);
        //player.StopCoroutine(plusStaminaCo);
    }

    public override void Update()
    {
       
    }

}
public class CaughtState: PlayerState // ºó»ç »óÅÂ
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