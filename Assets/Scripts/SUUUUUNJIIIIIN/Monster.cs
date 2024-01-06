using System.Collections;
using System.Collections.Generic;
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
    private bool isNextPosition;
    private bool isCheck;
    private bool isPlayerCheck;
    private float maxDistance;
    private float stunTime;
    private Stack<Transform> playerSoundPos;
    //public Collider[] PlayerHeardCol
    //{
    //    get => playerHeardCol;
    //    set => playerHeardCol = value;
    //}
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
        maxDistance = 10f;

        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = maxDistance;
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
        sm.curState.Update();
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            sm.SetState("Stun");
        }
        playerLookCol = Physics.OverlapSphere(transform.position, 5f, targetLayerMask);
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "coordinate")
            isNextPosition = true;
    }
    public IEnumerator MonsterMoveCo()
    {
        for (int i = 0; i < monsterNextPositionList.Count+1; i++)
        {
            if (i == monsterNextPositionList.Count)
                i = 0;
            while (!isNextPosition)
            {
                agent.SetDestination(monsterNextPositionList[i].transform.position);
                yield return null;
            }
            isNextPosition = false;
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