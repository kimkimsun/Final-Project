using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine.UI;

public class MainPlayer : MonoBehaviour
{
    public FirstPersonController playerMove;

    [SerializeField] private Inventory equipInventory;
    [SerializeField] private int hp;
    [SerializeField] private float stamina;
    private StateMachine<MainPlayer> playerSM;
    private int tension;
    private int slotIndexNum;

    //�׽�Ʈ���� �׸ŷ� �̻簡�ߵ�
    private Stack<Object> UIStack = new Stack<Object>();

    public float Stamina
    {
        get { return stamina; }
        set 
        { 
            stamina = value; 
            if(stamina >= 100)
            {
                stamina = 100;
                playerMove.MoveSpeed = 4.0f;
                playerMove.SprintSpeed = 6.0f;
            }
            if (stamina <= 0)
            {
                stamina = 0;
                playerMove.MoveSpeed = 2.5f;
                playerMove.SprintSpeed = 0f;
            }
        } 
    }
    public int Tension
    {
        get { return tension; }
        set 
        {
            tension = value;
            //�ټ��� ���Ϳ� ����ĥ�� �پ����
            if(tension <=50)
            {
                //UI ��ȭ
                //����Ҹ�
                playerSM.SetState("Exhaustion");
            }
        }
    }

    public int Hp
    { 
        get { return hp; } 
        set 
        {  
            hp = value;
            if (hp <=30)
            {
                playerSM.SetState("Moribund");
            }
            if (hp <= 0)
            {
                hp = 0;
                ScenesManager.Instance.Die();
            }
        } 
    }
    private void Start()
    {

        playerMove = GetComponent<FirstPersonController>();
        playerSM = new StateMachine<MainPlayer>();
        playerSM.owner = this;

        playerSM.AddState("Idle", new IdleState());
        playerSM.AddState("Exhaustion", new ExhaustionState());
        playerSM.AddState("Moribund", new MoribundState());
        playerSM.SetState("Idle");

        hp = 100;
        tension = 100;
        stamina = 100;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            playerSM.SetState("Exhaustion");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            playerSM.SetState("Moribund");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            playerSM.SetState("Idle");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!equipInventory.gameObject.activeSelf)
            {
                UIStack.Push(equipInventory);
                equipInventory.gameObject.SetActive(true);
            }
            else
                return;
        }
        if (equipInventory.gameObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                slotIndexNum--;
                if (slotIndexNum == -1)
                    slotIndexNum = equipInventory.EquipSlot.Length - 1;
                equipInventory.IndexSlot(slotIndexNum);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                slotIndexNum++;
                if (slotIndexNum == equipInventory.EquipSlot.Length)
                    slotIndexNum = 0;
                equipInventory.IndexSlot(slotIndexNum);
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (UIStack.Count > 0)
                UIStack.Pop().GameObject().SetActive(false);
            else
                Debug.Log("���⿡ ����â �����°� �ؾߵ˴ϴ�");
        }
    }
}