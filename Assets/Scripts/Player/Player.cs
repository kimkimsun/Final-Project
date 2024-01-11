using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using Unity.VisualScripting;
using CustomInterface;
using System.Collections;

public class Player : MonoBehaviour, IEventable
{
    public FirstPersonController playerMove;
    public FinalEvent finalEvent;
    private StateMachine<Player> playerSM;
    [SerializeField] private InteractionAim aim;

    public GameObject itemBox;
    public Inventory inven;
    public Inventory quickSlot;
    public Inventory portableInven;
    private LayerMask monsterMask;
    public List<ISubscribeable> eventObjs = new List<ISubscribeable>();
    [SerializeField] private int hp;
    [SerializeField] private float stamina;
    public int slotIndexNum;
    private static int exitItemCount;
    private int finalKey = 5;
    private int maxDistance = 5;
    private int tension;
    private int tensionDwon = 5;
    private int tensionUp = 3;
    private int max = 100;
    private int zero = 0;
    private bool isMonsterCheck = false;
    private IEnumerator minusTensionCo;
    private IEnumerator plusTensionCo;

    public Inventory Inven
    {
        get => inven;
    }

    public Inventory QuickSlot
    {
        get => quickSlot;
    }

    public bool IsMonsterCheck
    {
        get { return isMonsterCheck; }
        set 
        { 
            isMonsterCheck = value;
            if (isMonsterCheck)
            {
                StopCoroutine(plusTensionCo);
                StartCoroutine(minusTensionCo);
            }
            else
            {
                StopCoroutine(minusTensionCo);
                StartCoroutine(plusTensionCo);
            }
        }
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
            if (stamina >= max)
            {
                stamina = 100;
                playerMove.MoveSpeed = 4.0f;
                playerMove.SprintSpeed = 6.0f;
            }
            if (stamina <= zero)
            {
                stamina = zero;
                playerMove.MoveSpeed = 2.5f;
                playerMove.SprintSpeed = zero;
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
            if(tension <= max)
                tension = max;
            if (tension <= 60)
            {
                playerSM.SetState("Exhaustion");
            }
            if (tension >= 60)
            {
                playerSM.SetState("IdleState");
            }
        }
    }
    public int Hp
    {
        get { return hp; }
        set
        {
            hp = value;
            if (hp >= max)
                hp = max;
            if (hp <= 30)
            {
                playerSM.SetState("Moribund");
            }
            if (hp <= zero)
            {
                hp = zero;
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
        monsterMask = 1 << 9;

        minusTensionCo = MinusTensionCo(tensionDwon);
        plusTensionCo = PlusTensionCo(tensionUp);

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
    public IEnumerator MinusTensionCo(int damege)
    {
        while (Tension > zero)
        {
            yield return new WaitForSeconds(3);
            Hp -= damege;
            Debug.Log(damege);
        }
        yield break;
    }
    public IEnumerator PlusTensionCo(int tensionUp)
    {
        while (Tension < zero)
        {
            yield return new WaitForSeconds(5);
            Hp += tensionUp;
            yield return new WaitUntil(() => Hp < 100);
        }
    }

    private void Update()
    {

        Collider[] MonsterZoneCol = Physics.OverlapSphere(transform.position, 10, monsterMask);
        bool isMonsterZon = MonsterZoneCol.Length > 0;
        if (isMonsterZon)
        {
            RaycastHit hit;
            Vector3 direction = ((MonsterZoneCol[0].transform.position) - transform.position).normalized;
            Debug.DrawLine(transform.position, transform.position + (direction * maxDistance), Color.blue);
            if (Physics.Raycast(transform.position, direction, out hit, maxDistance))
            {
                IsMonsterCheck = hit.collider.gameObject.layer == monsterMask;
            }
        }
    }
}