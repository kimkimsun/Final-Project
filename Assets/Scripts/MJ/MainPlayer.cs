using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEditor.Experimental.GraphView;

public class MainPlayer : MonoBehaviour
{
    public FirstPersonController playerMove;

    [SerializeField] private Inventory equipInventory;
    [SerializeField] private int hp;
    [SerializeField] private float stamina;
    private StateMachine<MainPlayer> playerSM;
    private int tension;
    private int slotIndexNum;
    //테스트용임 겜매로 이사가야됨
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
            //텐션은 몬스터와 마주칠시 줄어들음
            if(tension <=50)
            {
                //UI 변화
                //심장소리
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
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!equipInventory.gameObject.activeSelf)
            {
                equipInventory.gameObject.SetActive(true);
                slotIndexNum = 4;
                equipInventory.IndexSlot(slotIndexNum);
                UIStack.Push(equipInventory);
            }
            else
                return;
        }
        if (equipInventory.gameObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                slotIndexNum--;
                if (slotIndexNum == 0)
                    slotIndexNum = equipInventory.EquipSlot.Length - 1;
                equipInventory.IndexSlot(slotIndexNum);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                slotIndexNum++;
                if (slotIndexNum == equipInventory.EquipSlot.Length)
                    slotIndexNum = 1;
                equipInventory.IndexSlot(slotIndexNum);
            }
        }
        if (equipInventory.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            equipInventory.SwitchItem();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (UIStack.Count > 0)
                UIStack.Pop().GameObject().SetActive(false);
            else
                Debug.Log("여기에 설정창 나오는거 해야됩니당");
        }
    }
}