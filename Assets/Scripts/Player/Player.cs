using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using Unity.VisualScripting;
using CustomInterface;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("인벤토리")]
    public Inventory oneSlot;
    public Inventory inven;
    public Inventory quickSlot;
    public Inventory portableInven;
    [Header("이스케이프 써클")]
    public Image escapeCircle;
    [Header("Etc.")]
    public FirstPersonController playerMove;
    public GameEvent finalEvent;
    public InteractionAim aim;
    public GameObject itemBox;

    private static int exitItemCount;
    private LayerMask monsterMask;
    [SerializeField] private int hp;
    [SerializeField] private float stamina;
    [SerializeField] private int tension;

    private StateMachine<Player> playerSM;
    private int tensionDwon = 5;
    private int tensionUp = 3;
    private int maxDistance = 5;
    private int max = 100;
    private int zero = 0;
    private int finalKey = 5;
    private bool isMonsterCheck;
    private bool isMonsterAttackCheck;
    private bool caughtSetState;
    private IEnumerator minusTensionCo;
    private IEnumerator plusTensionCo;
    public int slotIndexNum;

    #region 프로퍼티
    public Inventory Inven
    {
        get => inven;
    }

    public Inventory QuickSlot
    {
        get => quickSlot;
    }
    public bool CaughtSetState
    {
        get => caughtSetState;
        set => caughtSetState = value;
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
    public bool IsMonsterAttackCheck
    {
        get => isMonsterAttackCheck;
        set
        {
            isMonsterAttackCheck = value;
            if (isMonsterAttackCheck && caughtSetState)
            {
                playerSM.SetState("Caught");
                caughtSetState = false;
            }
        }
    }
    public int ExitItemCount
    {
        get { return exitItemCount; }
        set 
        {
            exitItemCount = value;
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
                stamina = max;
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
            if (tension <= 60) // && bool값 true로 있어야됨 false는 setstate밑 다시 true는 exit에서
            {
                playerSM.SetState("Exhaustion");
            }
            if (tension > 60)
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
    #endregion
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

        Hp = max;
        Tension = max;
        Stamina = max;
        monsterMask = 1 << 9;
        caughtSetState = true;

        minusTensionCo = MinusTensionCo(tensionDwon);
        plusTensionCo = PlusTensionCo(tensionUp);
        finalEvent.RegisterListener(() => { if (exitItemCount <= finalKey) ; });
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(transform.position.x,transform.position.y + 5, transform.position.z), 1f);
    }
    bool CheckInLayerMask(int layerIndex)
    {
        return (monsterMask & (1 << layerIndex)) != 0;
    }
    private void Update()
    {
        Collider[] monsterAttackZoneCol = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y + 5, transform.position.z), 1, monsterMask);
        bool isMonsterAttackZone = monsterAttackZoneCol.Length > 0;
        if(isMonsterAttackZone)
        {
            RaycastHit hit;
            Vector3 direction = ((monsterAttackZoneCol[0].transform.position) - transform.position).normalized;
            Debug.DrawLine(transform.position, transform.position + (direction * maxDistance), Color.blue);
            if (Physics.Raycast(transform.position, direction, out hit, maxDistance))
                IsMonsterAttackCheck = CheckInLayerMask(hit.collider.gameObject.layer);
        }

        Collider[] monsterZoneCol = Physics.OverlapSphere(transform.position, 10, monsterMask);
        bool isMonsterZone = monsterZoneCol.Length > 0;
        if (isMonsterZone)
        {
            RaycastHit hit;
            Vector3 direction = ((monsterZoneCol[0].transform.position) - transform.position).normalized;
            Debug.DrawLine(transform.position, transform.position + (direction * maxDistance), Color.blue);
            if (Physics.Raycast(transform.position, direction, out hit, maxDistance))
            {
                IsMonsterCheck = hit.collider.gameObject.layer == monsterMask;
            }
        }
    }
}