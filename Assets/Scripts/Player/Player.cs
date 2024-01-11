using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using Unity.VisualScripting;
using CustomInterface;

public class Player : MonoBehaviour, IEventable
{
    public FirstPersonController playerMove;
    public FinalEvent finalEvent;
    private StateMachine<Player> playerSM;
    [SerializeField] private InteractionAim aim;

    public GameObject itemBox;
    public Inventory inven;
    public Inventory quickSlot;
    public List<ISubscribeable> eventObjs = new List<ISubscribeable>();
    [SerializeField] private int hp;
    [SerializeField] private float stamina;
    public int slotIndexNum;
    private static int exitItemCount;
    private int finalKey = 5;
    private int tension;

    public Inventory Inven
    {
        get => inven;
    }

    public Inventory QuickSlot
    {
        get => quickSlot;
    }

    public int ExitItemCount
    {
        get { return exitItemCount; }
        set 
        {
            exitItemCount = value; 
            if(exitItemCount <= finalKey) 
            {
                Raise();
            }
        }
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
        playerSM.AddState("Caught", new CaughtState());
        playerSM.SetState("Idle");

        hp = 100;
        tension = 100;
        stamina = 100;

        aim.monsterCheck += () => {playerSM.SetState("Caught");};
    }

    public void Raise()
    {
        foreach(ISubscribeable eventObj in eventObjs)
        {
            eventObj.OnEvent();
        }
    }

    public void RegisterListener(ISubscribeable listener)
    {
        throw new System.NotImplementedException();
    }

    public void UnregisterListener(ISubscribeable listener)
    {
        throw new System.NotImplementedException();
    }
}