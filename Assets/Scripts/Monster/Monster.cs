using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    #region 변수
    [SerializeField] private GameObject map;
    [SerializeField] private LayerMask targetLayerMask;
    private IEnumerator escapeCo;
    private List<Transform> monsterNextPositionList;
    private StateMachine<Monster> sm;
    private NavMeshAgent agent;
    private Collider[] playerLookCol;
    private Collider[] playerHeardCol;
    private Collider[] playerAttackCol;
    private Rigidbody rb;
    private Animator animator;
    private bool isCheck;
    private bool isPlayerCheck;
    private bool isStun;
    private bool isAttack;
    public float escape;
    private float maxDistance;
    private float stunTime;
    private float? distance= null;
    private float tempDistance;
    private float extraRotationSpeed = 3f;
    private Stack<Transform> playerSoundPos;
    #endregion
    #region 프로퍼티
    //public Collider[] PlayerHeardCol
    //{
    //    get => playerHeardCol;
    //    set => playerHeardCol = value;
    //}
    public IEnumerator EscapeCor
    {
        get => escapeCo; 
        set => escapeCo = value;
    }

    public StateMachine<Monster> Sm
    {
        get => sm;
        set => sm = value;
    }
    public Animator Animator
    {
        get => animator;
        set => animator = value;
    }

    public Collider[] PlayerLookCol
    {
        get => playerLookCol;
        set => playerLookCol = value;
    }
    public Collider[] PlayerAttackCol
    {
        get => playerAttackCol;
        set => playerLookCol = value;
    }
    public NavMeshAgent Agent
    {
        get => agent;
        set { agent = value; }
    }
    public bool IsPlayerCheck
    {
        get { return isPlayerCheck; }

        set
        {
            isPlayerCheck = value;
            if (isPlayerCheck && isStun)
                sm.SetState("Run");
            else if (isPlayerCheck && isStun)
                sm.SetState("Idle");
        }

    }
    public bool IsCheck 
    {
        get => isCheck;
        set 
        { 
            isCheck = value;  
            if (!isCheck && isStun) 
                sm.SetState("Idle"); 
        } 
    }
    public Rigidbody Rb
    { get => rb; 
      set => rb = value;
    }
    #endregion
    private void OnEnable()
    {
        GameManager.Subscribe(FinalAttraction);
    }
    private void OnDisable()
    {
        GameManager.UnSubscribe(FinalAttraction);
    }
    private void FinalAttraction()
    {
        Debug.Log("출력되는지 확인");
        agent.SetDestination(GameManager.Instance.player.transform.position);
    }
    private void Awake()
    {
        playerSoundPos = new Stack<Transform>();
        monsterNextPositionList = new List<Transform>
        {
            map.transform.GetChild(0),
            map.transform.GetChild(1),
            map.transform.GetChild(2),
            map.transform.GetChild(3)
        };
    }
    private void Start()
    {
        escapeCo = EscapeCo();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        maxDistance = 20f;
        agent.speed = 5f;
        isStun = true;
        isAttack = true;
        sm = new StateMachine<Monster>();
        sm.owner = this;


        sm.AddState("Idle", new MonsterIdleState());
        sm.AddState("Run", new MonsterRunState());
        sm.AddState("Stun", new MonsterStunState());
        sm.AddState("Attack", new MonsterAttackState());
        sm.SetState("Idle");
    }

    bool CheckInLayerMask(int layerIndex)
    {
        return (targetLayerMask & (1 << layerIndex)) != 0;
    }
    private void FixedUpdate()
    {
        ResetRigidbody();
    }
    private void ResetRigidbody()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    public void MonsterAttack()
    {
        Debug.Log("탈출해 야발");
        StartCoroutine(escapeCo);
    }
    private void Update()
    {
        sm.curState?.Update();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            sm.SetState("Stun");
        }
        Vector3 lookrotation = agent.steeringTarget - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), extraRotationSpeed * Time.deltaTime);


        playerLookCol = Physics.OverlapSphere(transform.position, 10, targetLayerMask);
        playerAttackCol = Physics.OverlapSphere(transform.position, 2, targetLayerMask);
        IsCheck = playerLookCol.Length > 0;
        if (IsCheck)
        {
            RaycastHit hit;
            Vector3 direction = ((playerLookCol[0].transform.position) - transform.position).normalized;
            Debug.DrawLine(transform.position, transform.position + (direction * maxDistance), Color.blue);
            if (Physics.Raycast(transform.position, direction, out hit, maxDistance))
                IsPlayerCheck = CheckInLayerMask(hit.collider.gameObject.layer);
        }
        if (playerAttackCol.Length > 0 && isAttack)
        {
            sm.SetState("Attack");
            isAttack = false;
            isStun = false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 5f);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 10f);
        Gizmos.DrawRay(transform.position, Vector3.forward * 5f);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }
    public IEnumerator MonsterMoveCo()
    {
        yield return null;
        foreach (Transform targetPos in monsterNextPositionList)
        {
            if (distance == null)
                distance = Vector3.Distance(transform.position, targetPos.position);
            else
            {
                tempDistance = Vector3.Distance(transform.position, targetPos.position);
                if (distance > tempDistance)
                {
                    distance = tempDistance;
                    agent.SetDestination(targetPos.position);
                }
            }
        }
        for (int i = 0; i < monsterNextPositionList.Count+1; i++)
        {
            if (i == monsterNextPositionList.Count)
                i = 0;
            while (1 < Vector3.Distance(monsterNextPositionList[i].position, transform.position))
            {
                agent.SetDestination(monsterNextPositionList[i].transform.position);
                yield return null;
            }
        }
    }
    public IEnumerator StunCo()
    {
        isStun = false;
        stunTime = 0f;
        while(stunTime < 3.0f)
        {
            stunTime += Time.deltaTime;
            yield return null;
        }
        sm.SetState("Idle");
        isStun = true;
        isAttack = true;
    }
    public IEnumerator EscapeCo()
    {
        Debug.Log("들어옴");
        escape = 0;
        while (escape < 5)
        {
            escape += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.J))
            {
                sm.SetState("Stun");
            }
            yield return new WaitUntil(() => escape < 2);
        }
        Debug.Log("주금");
    }
}