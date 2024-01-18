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
        Debug.Log("±âº» »óÅÂ");
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


public class ExhaustionState : PlayerState //Å»Áø»óÅÂ
{
    
    public override void Enter()
    {
        Debug.Log("Å»Áø »óÅÂ");
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



public class MoribundState : PlayerState // ºó»ç »óÅÂ
{

    public override void Enter()
    {
        Debug.Log("ºó»ç »óÅÂ");
        damage = 10;
        minusHpCo = MinusHpCo(damage);
        player.playerMove.MoveSpeed = 1.0f;
        player.playerMove.SprintSpeed = 3.2f;
        player.StartCoroutine(minusHpCo);
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
public class CaughtState: PlayerState //¸ó½ºÅÍÇÑÅ× ÀâÇûÀ»¶§
{
    IEnumerator caughtCo;
    Image diecount;
    int item;
    int itemcount;
    public override void Enter()
    {
        Init();
        player.playerMove.enabled = false;
        player.StartCoroutine(caughtCo);
    }

    public void Init()
    {
        caughtCo = CaughtCo();
        diecount = UIManager.Instance.escapeCircle;
        item = GameManager.Instance.player.quickSlot.HairPinSlot.items.Count;
        itemcount = GameManager.Instance.player.quickSlot.HairPinSlot.items.Count - 1;
    }
    protected IEnumerator CaughtCo()
    {
        diecount.fillMethod = 0;
        while (diecount.fillAmount <= 1)
        {
            yield return null;
            diecount.fillAmount += (Time.deltaTime/ 2);
            if (item > 0)
            {
                diecount.gameObject.SetActive(true);
                if (diecount.fillAmount <= 0.6f)
                    diecount.GetComponent<Image>().color = new Color(0, 0, 0, 1);
                else if (diecount.fillAmount > 0.6f)
                {
                    diecount.GetComponent<Image>().color = new Color(0, 1, 0, 1);
                    yield return new WaitForSeconds(3);
                    if (item == itemcount)
                        Exit();
                }

            }
            else
                ScenesManager.Instance.DieScene();
        }
        ScenesManager.Instance.DieScene();

    }
    public override void Exit()
    {
        player.StopCoroutine(caughtCo);
        player.playerMove.enabled = true;
        player.Tension = 50;
        Debug.Log("Å»Ãâ ¼º°ø");
    }

    public override void Update()
    {

    }
}