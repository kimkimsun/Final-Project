using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        Debug.Log("�������?");
        while (player.Hp > 0)
        {
            yield return new WaitForSeconds(1);
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

    public override void Update() { }
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

    public override void Update() { }
}
public class CaughtState: PlayerState //�������� ��������
{
    IEnumerator caughtCo;
    Image diecount;
    int item;
    int itemcount;
    bool isUse;
    public override void Enter()
    {
        Debug.Log("��������");
        Init();
        player.quickSlot.HairPinSlot.OnUse += () => isUse = true;
        player.playerMove.enabled = false;
        if (item > 0)
            player.StartCoroutine(CaughtCo());
        else
            ScenesManager.Instance.DieScene();
    }

    public void Init()
    {
        caughtCo = CaughtCo();
        diecount = UIManager.Instance.escapeCircle;
        item = GameManager.Instance.player.quickSlot.HairPinSlot.items.Count;
    }
    protected IEnumerator CaughtCo()
    {
        diecount.gameObject.SetActive(true);
        while (diecount.fillAmount < 1)
        {
            yield return null;
            diecount.fillAmount += (Time.deltaTime / 2);
            if (diecount.fillAmount <= 0.6f)
                diecount.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            else if (diecount.fillAmount > 0.6f)
            {
                diecount.GetComponent<Image>().color = new Color(0, 1, 0, 1);
                if (isUse)
                {
                    yield return new WaitForSeconds(1);
                    Exit();
                }
                    
            }
        }
        ScenesManager.Instance.DieScene();
    }
    public override void Exit()
    {
        player.playerMove.enabled = true;
        player.Tension = 50;
        diecount.gameObject.SetActive(false);
        diecount.fillAmount = 0;
        Debug.Log("Ż�� ����");
    }

    public override void Update() { }
}