using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    [SerializeField] List<GameObject> coordinatePosition;
    StateMachine<Monster> sm;
    NavMeshAgent agent;
    public bool test;
    private void Start()
    {
        coordinatePosition = new List<GameObject>();
        agent = GetComponent<NavMeshAgent>();
        sm = new StateMachine<Monster>();
        sm.owner = this;

        sm.AddState("Idle", new MonsterIdleState());
        sm.AddState("Run", new MonsterRunState());
        sm.SetState("Idle");
    }
    private void Update()
    {

        Collider[] cols = Physics.OverlapSphere(transform.position, 5f, 1 << 10);
        if (cols.Length > 0)
        {
            agent.SetDestination(cols[0].transform.position);
        }
        else
            Debug.Log("พฦดิ");
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 5f);
        Gizmos.DrawRay(transform.position, Vector3.forward * 5f);
    }
    public IEnumerator MonsterMoveCo()
    {
        for (int i = 0; i < coordinatePosition.Count; i++)
        {
            while ((transform.position == coordinatePosition[i].transform.position))
            {
                agent.SetDestination(coordinatePosition[0].transform.position);
                yield return null;
            }
            i++;
        }

    }
}