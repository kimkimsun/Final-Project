//using System.Collections.Generic;
//using UnityEngine;
//using StarterAssets;
//using Unity.VisualScripting;

//public class PlayerTest : MonoBehaviour
//{
//    public FirstPersonController playerMove;
//    public FinalEvent finalEvent;
//    private StateMachine<Player> playerSM;
//    //�� ���
//    [SerializeField] private LayerMask monsterLayerMask;
//    public GameObject itemBox;
//    public Inventory inven;
//    public Inventory quickSlot;
//    [SerializeField] private int hp;
//    [SerializeField] private float stamina;
//    public int slotIndexNum;
//    private int tension;
//    private int a;
//    //�׽�Ʈ���� �׸ŷ� �̻簡�ߵ�

//    /*    public int A
//        {
//            get => a;
//            set
//            {
//                a = value;
//                if (a >= 5)
//                {
//                    finalEvent.Raise();
//                }
//            }
//        }*/
//    public Inventory Inven
//    {
//        get => inven;
//    }

//    public Inventory QuickSlot
//    {
//        get => quickSlot;
//    }


//    public float Stamina
//    {
//        get { return stamina; }
//        set
//        {
//            stamina = value;
//            if (stamina >= 100)
//            {
//                stamina = 100;
//                playerMove.MoveSpeed = 4.0f;
//                playerMove.SprintSpeed = 6.0f;
//            }
//            if (stamina <= 0)
//            {
//                stamina = 0;
//                playerMove.MoveSpeed = 2.5f;
//                playerMove.SprintSpeed = 0f;
//            }
//        }
//    }
//    public int Tension
//    {
//        get { return tension; }
//        set
//        {
//            tension = value;
//            //�ټ��� ���Ϳ� ����ĥ�� �پ����
//            if (tension <= 50)
//            {
//                //UI ��ȭ
//                //����Ҹ�
//                playerSM.SetState("Exhaustion");
//            }
//        }
//    }
//    public int Hp
//    {
//        get { return hp; }
//        set
//        {
//            hp = value;
//            if (hp <= 30)
//            {
//                playerSM.SetState("Moribund");
//            }
//            if (hp <= 0)
//            {
//                hp = 0;
//                ScenesManager.Instance.Die();
//            }
//        }
//    }

//    private void Start()
//    {
//        playerMove = GetComponent<FirstPersonController>();
//        playerSM = new StateMachine<Player>();
//        playerSM.owner = this;

//        playerSM.AddState("Idle", new IdleState());
//        playerSM.AddState("Exhaustion", new ExhaustionState());
//        playerSM.AddState("Moribund", new MoribundState());
//        playerSM.SetState("Idle");

//        hp = 100;
//        tension = 100;
//        stamina = 100;
//    }

//    private void Update()
//    {
//        Collider[] playerHitCol = Physics.OverlapSphere(transform.position, 2, monsterLayerMask);

//        if(playerHitCol.Length > 0 )
//        {
//            if()
//        }
//        /*        if (Input.GetKeyDown(KeyCode.Keypad0))
//                {
//                    A++;
//                }*/
//    }
//}