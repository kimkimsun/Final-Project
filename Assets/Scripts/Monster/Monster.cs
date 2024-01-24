using Cinemachine;
using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class Monster : MonoBehaviour, IGetStunable
{
    #region 변수
    public float escape;
    [SerializeField] protected LayerMask targetLayerMask;
    [SerializeField] protected LayerMask heardTargetLayerMask;
    [SerializeField] protected GameObject map;
    [SerializeField] protected GameEvent pauseEvent;
    [SerializeField] protected GameEvent finalEvent;
    [SerializeField] protected NavMeshAgent agent;
    public Transform monsterPos;
    protected Door door;
    protected StateMachine<Monster> sm;
    protected Transform footTrans;
    protected List<Transform> monsterNextPositionList;
    protected Collider[] playerLookCol;
    protected Collider[] soundCol;
    protected Rigidbody rb;
    protected Animator animator;
    protected IEnumerator escapeCor;
    protected bool isHeardCheck;
    protected bool isCheck;
    protected bool isPlayerCheck;
    protected bool isStun;
    protected float maxDistance;
    protected float stunTime;
    protected float? distance = null;
    protected float tempDistance;
    protected float extraRotationSpeed;
    protected int playerFootLayerNum;
    protected int lookDetectionRange;
    protected int attackDetectionRange;
    #endregion
    #region 프로퍼티
    public bool IsHeardCheck
    {
        get => isHeardCheck;
        set => IsHeardCheck = value;
    }
    public Collider[] SoundCol
    {
        get => soundCol;
        set => soundCol = value;
    }
    public IEnumerator EscapeCor
    {
        get => escapeCor;
        set => escapeCor = value;
    }
    public Transform FootTrans
    {
        get => footTrans; set => footTrans = value;
    }
    public float Escape
    {
        get => escape;
        set => escape = value;
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
    public NavMeshAgent Agent
    {
        get => agent;
        set { agent = value; }
    }
    public bool IsPlayerCheck
    {
        get { return isPlayerCheck; }
        set => isPlayerCheck = value;
    }

    public Rigidbody Rb
    {
        get => rb;
        set => rb = value;
    }
    #endregion
    protected void OnEnable()
    {
        pauseEvent.UnregisterListener(Play);
        pauseEvent.RegisterListener(Stop);
    }
    protected void OnDisable()
    {
        pauseEvent.UnregisterListener(Stop);
        pauseEvent.RegisterListener(Play);
    }
    protected void Awake()
    {
        monsterNextPositionList = new List<Transform>();
        for (int i = 0; i < map.transform.childCount; i++)
        {
            monsterNextPositionList.Add(map.transform.GetChild(i));
        }
    }

    protected bool CheckInLayerMask(int layerIndex)
    {
        return (targetLayerMask & (1 << layerIndex)) != 0;
    }
    
    protected void Stop()
    {
        agent.isStopped = true;
        enabled = false;
        animator.enabled = false;
        this.enabled = false;
    }
    protected void Play()
    {
        agent.isStopped = false;
        this.enabled = true;
        enabled = true;
        animator.enabled = true;
    }
    protected void PublicUpdate()
    {
        playerLookCol = Physics.OverlapSphere(transform.position, lookDetectionRange, targetLayerMask);
    }
    protected void PublicStart()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        escapeCor = EscapeCo();
        playerFootLayerNum = 8;
        extraRotationSpeed = 3f;
        attackDetectionRange = 1;
    }
    protected void FixedUpdate()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        Vector3 lookrotation = (agent.steeringTarget - transform.position);
        if (lookrotation != Vector3.zero)
        {
            Vector3 lookratationDirection = lookrotation.normalized;

            if (lookratationDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookratationDirection), extraRotationSpeed * Time.deltaTime);
            }
        }
    }
    public void MonsterAttack()
    {
        StartCoroutine(StunCo());
    }
    protected virtual void Start()
    {
        PublicStart();
    }

    protected virtual void Update()
    {
        PublicUpdate();
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookDetectionRange);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, attackDetectionRange);
    }
    public virtual IEnumerator MonsterMoveCo()
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
        for (int i = 0; i < monsterNextPositionList.Count + 1; i++)
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
    public abstract IEnumerator StunCo();
    public IEnumerator EscapeCo()
    {
        while (escape < 5)
        {
            escape += Time.deltaTime;
            yield return new WaitUntil(() => escape < 5);
        }
    }

    public abstract void GetStun();
}