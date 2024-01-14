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
    [SerializeField] private LayerMask heardTargetLayerMask;
    [SerializeField] private GameObject target;
    [SerializeField] private CinemachineVirtualCamera monsterVirtualCamera;
    [SerializeField] private GameEvent pauseEvent;
    [SerializeField] private GameEvent finalEvent;
    private IEnumerator escapeCo;
    private Transform footTrans = null;
    private List<Transform> monsterNextPositionList;
    private StateMachine<Monster> sm;
    private NavMeshAgent agent;
    private Collider[] playerLookCol;
    private Collider[] playerHeardCol;
    private Collider[] playerAttackCol;
    private Collider[] heardCol;
    private Rigidbody rb;
    private Animator animator;
    private bool isCheck;
    private bool isPlayerCheck;
    private bool isStun;
    private bool isAttack;
    private bool isHeardCheck;
    public float escape;
    private float maxDistance;
    private float stunTime;
    private float? distance= null;
    private float tempDistance;
    private float extraRotationSpeed = 3f;
    private int playerFootLayerNum = 8;
    #endregion
    #region 프로퍼티
    //public Collider[] PlayerHeardCol
    //{
    //    get => playerHeardCol;
    //    set => playerHeardCol = value;
    //}
    public Transform FootTrans
    {
        get => footTrans; set => footTrans = value;
    }
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
    public Collider[] HeardCol
    {
        get => heardCol;
        set => heardCol = value;
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
    public bool IsHeardCheck
    {
        get => isHeardCheck;
        set => isHeardCheck = value;
    }
    public bool IsPlayerCheck
    {
        get { return isPlayerCheck; }
        set => isPlayerCheck = value;
    }

    public Rigidbody Rb
    { get => rb; 
      set => rb = value;
    }
    #endregion
    private void OnEnable()
    {
        pauseEvent.UnregisterListener(Play);
        pauseEvent.RegisterListener(Stop);
    }
    private void OnDisable()
    {
        pauseEvent.UnregisterListener(Stop);
        pauseEvent.RegisterListener(Play);
    }
    private void FinalAttraction()
    {
        agent.SetDestination(GameManager.Instance.player.transform.position);
    }
    private void Awake()
    {
        monsterNextPositionList = new List<Transform>
        {
            map.transform.GetChild(0),
            map.transform.GetChild(1),
            map.transform.GetChild(2),
            map.transform.GetChild(3)
        };
    }
    
    bool CheckInLayerMask(int layerIndex)
    {
        return (targetLayerMask & (1 << layerIndex)) != 0;
    }

    private void Stop()
    {
        agent.isStopped = true;
        enabled = false;
        animator.enabled = false;
        this.enabled = false;
    }

    private void Play()
    {
        this.enabled = true;
        agent.isStopped = false;
        enabled = true;
        animator.enabled = true;
    }


    private void FixedUpdate()
    {
        ResetRigidbody();
        Vector3 lookrotation = agent.steeringTarget - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), extraRotationSpeed * Time.deltaTime);
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
    public void LayerCheckMethod() 
    {
        if (heardCol[0].gameObject.layer == playerFootLayerNum)
        {
            FootTrans = heardCol[0].gameObject.transform;
            heardCol[0] = null;
            if (sm.curState is MonsterIdleState mi && isStun)
                sm.SetState("Run");
        }
    }
    private void Start()
    {
        escapeCo = EscapeCo();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        maxDistance = 10f;
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

    private void Update()
    {
        sm.curState?.Update();

        playerLookCol = Physics.OverlapSphere(transform.position, 15, targetLayerMask);
        playerAttackCol = Physics.OverlapSphere(transform.position, 1, targetLayerMask);
        heardCol = Physics.OverlapSphere(transform.position, 15, heardTargetLayerMask);
        isHeardCheck = heardCol.Length > 0;
        if (heardCol.Length > 0 && sm.curState is MonsterIdleState mr && isStun && heardCol[0].gameObject.layer == 8)
        {
            footTrans = heardCol[0].gameObject.transform;
            heardCol[0] = null;
            sm.SetState("Run");
        }
        else if (playerLookCol.Length > 0)
        {
            RaycastHit hit;
            Vector3 direction = ((playerLookCol[0].transform.position) - transform.position).normalized;
            Debug.DrawLine(transform.position, transform.position + (direction * maxDistance), Color.blue);
            if (Physics.Raycast(transform.position, direction, out hit, maxDistance))
                isPlayerCheck = CheckInLayerMask(hit.collider.gameObject.layer);
            if (isPlayerCheck && sm.curState is MonsterIdleState ms && isStun)
                sm.SetState("Run");
            else if (!isPlayerCheck)
                sm.SetState("Idle");
            else
                return;
        }
        else
            sm.SetState("Idle");
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
        Gizmos.DrawWireSphere(transform.position, 15f);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 10f);

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