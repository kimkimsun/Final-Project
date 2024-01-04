using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    [SerializeField]
    private GameObject map;
    private List<Transform> monsterNextPosition;
    private StateMachine<Monster> sm;
    private NavMeshAgent agent;
    private bool isNextPosition;
    private bool isPlayerCheck;
    private Collider[] cols;


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
        agent = GetComponent<NavMeshAgent>();
        sm = new StateMachine<Monster>();
        sm.owner = this;

        sm.AddState("Idle", new MonsterIdleState());
        sm.AddState("Run", new MonsterRunState());
        sm.SetState("Idle");
    }
    private void Update()
    {
        sm.curState.Update();
        cols = Physics.OverlapSphere(transform.position, 5f, 1 << 10);
        if (cols.Length > 0)
        {
            Debug.Log("µé¾î¿È");
            if (!IsPlayerCheck)
                IsPlayerCheck = true;
            else
                return;
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