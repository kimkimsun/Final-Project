using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerNavigation : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject finalCoordination;

    public GameEvent finalEvent;

    private void Start()
    {
        gameObject.SetActive(false);

        finalEvent.RegisterListener(() => { gameObject.SetActive(true); });
        finalEvent.RegisterListener(() => { transform.SetParent(null); });
        finalEvent.RegisterListener(() => { agent.SetDestination(finalCoordination.transform.position); });
    }
}
