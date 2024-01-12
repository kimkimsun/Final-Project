using Cinemachine;
using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    #region 변수
    [SerializeField] private GameObject map;
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private CinemachineVirtualCamera monsterVirtualCamera;
    [SerializeField] private GameEvent pauseEvent;
    [SerializeField] private GameEvent finalEvent;
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
    public CinemachineVirtualCamera MonsterVirtualCamera
    {
        get => monsterVirtualCamera;
        set => monsterVirtualCamera = value;
    }
    public float Escape
    {
        get => escape;
        set => escape = value;
    }
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
        //pauseEvent.RegisterListener(() => {; });
        //finalEvent += () => {; };
    }
    private void OnDisable()
    {
        //pauseEvent -= () => {; };
        //finalEvent -= () => {; };
    }
    private void FinalAttraction()
    {
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
        StartCoroutine(escapeCo);
    }
    private void Update()
    {
        sm.curState?.Update();
        if (Input.GetKeyDown(KeyCode.Alpha1))
            sm.SetState("Stun");
        if (Input.GetKeyDown(KeyCode.Alpha7))
            monsterVirtualCamera.Priority = 11;
        Vector3 lookrotation = agent.steeringTarget - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), extraRotationSpeed * Time.deltaTime);

        playerLookCol = Physics.OverlapSphere(transform.position, 10, targetLayerMask);
        playerAttackCol = Physics.OverlapSphere(transform.position, 1, targetLayerMask);
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IStunable>(out IStunable stun))
            sm.SetState("Stun");
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 5f);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 10f);
        Gizmos.DrawRay(transform.position, Vector3.forward * 5f);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, 1f);
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
        while(stunTime < 5.0f)
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
        while (escape < 5)
        {
            escape += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.J))
            {
                sm.SetState("Stun");
            }
            yield return new WaitUntil(() => escape < 5);
        }
    }
}