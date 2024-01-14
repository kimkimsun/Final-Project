using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HaiKen : Monster
{
    private List<Transform> monsterNextPositionList;
    [SerializeField] private GameObject mapcoordi;
    private NavMeshAgent agent;
    private void Awake()
    {
        monsterNextPositionList = new List<Transform>
        {
            mapcoordi.transform.GetChild(0),
        };
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(monsterNextPositionList[0].transform.position);
    }
}
