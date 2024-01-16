using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class PlayerState : State
{
    public Player player;

    protected int damage;
    protected IEnumerator minusHpCo;

    public override void Init(IStateMachine sm)
    {
        this.sm = sm;
        player = (Player)sm.GetOwner();
    }

    protected IEnumerator MinusHpCo(int damage)
    {
        while (player.Hp > 0)
        {
            yield return new WaitForSeconds(3);
            player.Hp -= damage;
            Debug.Log(damage);
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
        damage = 5;
        minusHpCo = MinusHpCo(damage);
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
        damage = 10;
        minusHpCo = MinusHpCo(damage);
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
    IEnumerator caughtCo;
    private float a;
    public override void Enter()
    {
        Debug.Log("한번만 제발");
        player.playerMove.enabled = false;
/*        if (player.aim.isLookMonster && player.oneSlot.Slots[0].items.Count > 0)
        {
            player.StartCoroutine(CaughtCo());
        }
        else
            Debug.Log("주금");*/
    }
    protected IEnumerator CaughtCo()
    {
        while (UIManager.Instance.escapeCircle.fillAmount < 1)
        {
            yield return null;
            if (UIManager.Instance.escapeCircle.fillAmount < 0.6f)
            {
                UIManager.Instance.escapeCircle.GetComponent<Image>().color = new Color(0, 0, 0, 1);
               
            }
            else if (UIManager.Instance.escapeCircle.fillAmount > 0.6f)
            {
                UIManager.Instance.escapeCircle.GetComponent<Image>().color = new Color(0, 1, 0, 1);
            }
            UIManager.Instance.escapeCircle.gameObject.SetActive(true);
            UIManager.Instance.escapeCircle.fillAmount += (Time.deltaTime/ 2);
        }
    }
    public override void Exit()
    {
        player.playerMove.enabled = true;
        player.Tension = 50;
        Debug.Log("탈출 성공");
    }

    public override void Update()
    {

    }
}