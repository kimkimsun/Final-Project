using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    public FirstPersonController playerMove;
    public FinalEvent finalEvent;
    private StateMachine<Player> playerSM;

    public GameObject itemBox;
    public Inventory inven;
    public Inventory quickSlot;
    [SerializeField] private int hp;
    [SerializeField] private float stamina;
    private int tension;
    private int slotIndexNum;
    private int a;
    //테스트용임 겜매로 이사가야됨
    private Stack<Object> UIStack = new Stack<Object>();

    public int A
    {
        get => a;
        set
        {
            a = value;
            if (a >= 5)
            {
                finalEvent.Raise();
            }
        }
    }
    public Inventory Inven
    {
        get => inven;
    }

    public Inventory QuickSlot
    {
        get => quickSlot;
    }


    public float Stamina
    {
        get { return stamina; }
        set
        {
            stamina = value;
            if (stamina >= 100)
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
            if (tension <= 50)
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
            if (hp <= 30)
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
        playerSM = new StateMachine<Player>();
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
        #region 플레이어 상호작용 키
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            A++;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!inven.gameObject.activeSelf)
            {
                inven.gameObject.SetActive(true);
                slotIndexNum = 4;
                inven.IndexSlot(slotIndexNum);
                UIStack.Push(inven);
            }
            else
                return;
        }
        if (inven.gameObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Debug.Log(slotIndexNum);
                slotIndexNum--;
                if (slotIndexNum == -1)
                    slotIndexNum = inven.EquipQuickSlot.Length - 1;
                inven.IndexSlot(slotIndexNum);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                slotIndexNum++;
                if (slotIndexNum == inven.EquipQuickSlot.Length)
                    slotIndexNum = 1;
                inven.IndexSlot(slotIndexNum);
            }
        }
        else
            return;
        if (inven.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            inven.SwitchItem();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (UIStack.Count > 0)
                UIStack.Pop().GameObject().SetActive(false);
            else
                Debug.Log("여기에 설정창 나오는거 해야됩니당");
        }
        #endregion
    }
}