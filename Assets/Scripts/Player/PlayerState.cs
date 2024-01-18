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



public class MoribundState : PlayerState // ��� ����
{

    public override void Enter()
    {
        Debug.Log("��� ����");
        damage = 10;
        minusHpCo = MinusHpCo(damage);
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
    IEnumerator caughtCo;
    private float a;
    public override void Enter()
    {
        player.playerMove.enabled = false;
        player.StartCoroutine(CaughtCo());
    }
    protected IEnumerator CaughtCo()
    {
        while (UIManager.Instance.escapeCircle.fillAmount <= 1)
        {
            UIManager.Instance.openUI.color = new Color(0, 0, 0, 1);
            yield return new WaitForSeconds(1);

            yield return null;
            UIManager.Instance.escapeCircle.fillAmount += (Time.deltaTime/ 2);
            if (GameManager.Instance.player.quickSlot.HairPinSlot.items.Count > 0)
            {
                UIManager.Instance.escapeCircle.gameObject.SetActive(true);
                if (UIManager.Instance.escapeCircle.fillAmount <= 0.6f)
                    UIManager.Instance.escapeCircle.GetComponent<Image>().color = new Color(0, 0, 0, 1);
                else if (UIManager.Instance.escapeCircle.fillAmount > 0.6f)
                    UIManager.Instance.escapeCircle.GetComponent<Image>().color = new Color(0, 1, 0, 1);
            }
            else
                GameManager.Instance.BadEnding();
        }
    }
    public override void Exit()
    {
        player.playerMove.enabled = true;
        player.Tension = 50;
        Debug.Log("Ż�� ����");
    }

    public override void Update()
    {

    }
}