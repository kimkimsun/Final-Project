using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    [SerializeField] private GameObject map;
    [SerializeField] private LayerMask targetLayerMask;
    private List<Transform> monsterNextPositionList;
    private StateMachine<Monster> sm;
    private NavMeshAgent agent;
    private Collider[] playerLookCol;
    private Collider[] playerHeardCol;
    private Rigidbody rb;
    private Animator animator;
    private bool isNextPosition;
    private bool isCheck;
    private bool isPlayerCheck;
    private float maxDistance;
    private float stunTime;
    private float? distance= null;
    private float tempDistance;
    private float extraRotationSpeed = 3f;
    private Stack<Transform> playerSoundPos;
    //public Collider[] PlayerHeardCol
    //{
    //    get => playerHeardCol;
    //    set => playerHeardCol = value;
    //}
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
            if (isPlayerCheck)
                sm.SetState("Run");
            else if (!isPlayerCheck)
                sm.SetState("Idle");
        }

    }
    public bool IsCheck
    { get { return isCheck; } 
      set { isCheck = value; }
    }
    public Rigidbody Rb
    { get => rb; 
      set => rb = value;
    }
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
        maxDistance = 20f;

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 5;


        sm = new StateMachine<Monster>();
        sm.owner = this;
        sm.AddState("Idle", new MonsterIdleState());
        sm.AddState("Run", new MonsterRunState());
        sm.AddState("Stun", new MonsterStunState());
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
        IsCheck = playerLookCol.Length > 0;
        if (IsCheck)
        {
            RaycastHit hit;
            Vector3 direction = ((playerLookCol[0].transform.position) - transform.position).normalized;
            Debug.DrawLine(transform.position, transform.position + (direction * maxDistance), Color.blue);
            if (Physics.Raycast(transform.position, direction, out hit, maxDistance))
            {
                IsPlayerCheck = CheckInLayerMask(hit.collider.gameObject.layer);
            }
        }
        else
        {
            if (IsPlayerCheck)
                IsPlayerCheck = false;
            else
                return;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 5f);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 10f);
        Gizmos.DrawRay(transform.position, Vector3.forward * 5f);
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
        stunTime = 0f;
        while(stunTime < 3.0f)
        {
            stunTime += Time.deltaTime;
            yield return null;
        }
        sm.SetState("Idle");
    }
}