using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    [SerializeField] private GameObject map;
    [SerializeField] private LayerMask targetLayerMask;
    private List<Transform> monsterNextPosition;
    private StateMachine<Monster> sm;
    private NavMeshAgent agent;
    private bool isNextPosition;
    private bool isCheck;
    private bool isPlayerCheck;
    private Collider[] cols;
    private float maxDistance;


    public Collider[] Cols
    {
        get => cols;
        set => cols = value;
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

    private void Awake()
    {
        monsterNextPosition = new List<Transform>
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
        agent = GetComponent<NavMeshAgent>();
        sm = new StateMachine<Monster>();
        sm.owner = this;

        sm.AddState("Idle", new MonsterIdleState());
        sm.AddState("Run", new MonsterRunState());
        sm.SetState("Idle");
    }
    bool CheckInLayerMask(int layerIndex)
    {
        Debug.Log("비트연산자 계산 값" + (targetLayerMask & (1 << layerIndex)));
        return (targetLayerMask & (1 << layerIndex)) != 0;
    }
    private void Update()
    {
        sm.curState.Update();
        cols = Physics.OverlapSphere(transform.position, 5f, targetLayerMask);
        IsCheck = cols.Length > 0;
        if (IsCheck)
        {
            Debug.Log("들어옴");
            RaycastHit hit;
            Vector3 direction = ((cols[0].transform.position) - transform.position).normalized;
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
        Gizmos.DrawWireSphere(transform.position, 5f);
        Gizmos.DrawRay(transform.position, Vector3.forward * 5f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "coordinate")
            isNextPosition = true;
    }
    public IEnumerator MonsterMoveCo()
    {
        for (int i = 0; i < monsterNextPosition.Count; i++)
        {
            Debug.Log(monsterNextPosition.Count);
            while (!isNextPosition)
            {
                agent.SetDestination(monsterNextPosition[i].transform.position);
                yield return null;
            }
            isNextPosition = false;
        }
    }
}