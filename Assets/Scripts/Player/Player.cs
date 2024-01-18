using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using Unity.VisualScripting;
using CustomInterface;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using System;

[Serializable]
public class Player : MonoBehaviour
{
    [SerializeField] private static int exitItemCount;

    #region �÷��̾� ����
    [Header("�κ��丮")]
    public EquipItemInventory equipInven;
    public UseItemInventory quickSlot;
    public Inventory portableInven;

    public InteractionAim aim;
    public FirstPersonController playerMove;

    public Transform playerPos;
    private LayerMask monsterMask;
    public GameEvent finalEvent;
    public GameObject itemBox;

    [SerializeField] private int hp;
    [SerializeField] private float stamina;
    [SerializeField] private int tension;

    private StateMachine<Player> playerSM;
    private bool isHpCoStart;
    private int tensionDwon = 5;
    private int tensionUp = 3;
    private int maxDistance;
    private int max = 100;
    private int zero = 0;
    private int finalKey = 5;
    private int monsterLookZone;
    private bool isMonsterCheck;
    private bool isMonsterAttackCheck;
    private bool caughtSetState;
    private IEnumerator minusTensionCo;
    private IEnumerator plusTensionCo;
    private IEnumerator hpPlusCo;
    #endregion


    #region ������Ƽ

    public int MonsterLookZone
    {
        get => monsterLookZone;
        set => monsterLookZone = value;
    }
    public EquipItemInventory EquipInven
    {
        get => equipInven;
    }

    public Inventory QuickSlot
    {
        get => quickSlot;
    }
    public bool CaughtSetState
    {
        get => caughtSetState;
        set => caughtSetState = value;
    }

    public bool IsHpCoStart
    {
        get => isHpCoStart;
        set
        {
            isHpCoStart = value;
            if (isHpCoStart)
                StartCoroutine(hpPlusCo);
            else
                StopCoroutine(hpPlusCo);
        }
        

    }
    public bool IsMonsterCheck
    {
        get { return isMonsterCheck; }
        set 
        { 
            isMonsterCheck = value;
            if (isMonsterCheck)
            {
                StopCoroutine(plusTensionCo);
                StartCoroutine(minusTensionCo);
            }
            else
            {
                StopCoroutine(minusTensionCo);
                StartCoroutine(plusTensionCo);
            }
        }
    }
    public bool IsMonsterAttackCheck
    {
        get => isMonsterAttackCheck;
        set
        {
            isMonsterAttackCheck = value;
            if (isMonsterAttackCheck && caughtSetState)
            {
                playerSM.SetState("Caught");
                caughtSetState = false;
            }
        }
    }
    public int ExitItemCount
    {
        get { return exitItemCount; }
        set 
        {
            exitItemCount = value;
        }
    }
    public float Stamina
    {
        get { return stamina; }
        set
        {
            stamina = value;
            if (stamina >= max)
            {
                stamina = max;
                playerMove.MoveSpeed = 4.0f;
                playerMove.SprintSpeed = 6.0f;
            }
            if (stamina <= zero)
            {
                stamina = zero;
                playerMove.MoveSpeed = 2.5f;
                playerMove.SprintSpeed = zero;
            }
        }
    }
    public int Tension
    {
        get { return tension; }
        set
        {
            tension = value;
            //�ټ��� ���Ϳ� ����ĥ�� �پ����
            if(tension <= max)
                tension = max;
            if (tension <= 60) // && bool�� true�� �־�ߵ� false�� setstate�� �ٽ� true�� exit����
            {
                playerSM.SetState("Exhaustion");
            }
            if (tension > 60)
            {
                playerSM.SetState("IdleState");
            }
        }
    }
    public int Hp
    {
        get { return hp; }
        set
        {
            hp = value;
            if (hp >= max)
                hp = max;
            if (hp <= 30)
            {
                playerSM.SetState("Moribund");
            }
            if (hp <= zero)
            {
                hp = zero;
                ScenesManager.Instance.DieScene();
            }
        }
    }
    #endregion

    private void Start()
    {
        playerPos = GetComponent<Transform>();
        playerMove = GetComponent<FirstPersonController>();
        playerSM = new StateMachine<Player>();
        playerSM.owner = this;
        this.gameObject.transform.position = playerPos.position;

        playerSM.AddState("Idle", new IdleState());
        playerSM.AddState("Exhaustion", new ExhaustionState());
        playerSM.AddState("Moribund", new MoribundState());
        playerSM.AddState("Caught", new CaughtState());
        playerSM.SetState("Idle");
        
        Hp = max;
        Tension = max;
        Stamina = max;
        monsterMask = 1 << 9;
        caughtSetState = true;
        monsterLookZone = 10;
        maxDistance = 5;

        minusTensionCo = MinusTensionCo(tensionDwon);
        plusTensionCo = PlusTensionCo(tensionUp);
        hpPlusCo = HpPlusCo();
        finalEvent.RegisterListener(() => { this.enabled = true; });
    }
    public IEnumerator HpPlusCo()
    {
        while(hp <= max)
        {
            yield return new WaitForSeconds(30);
            hp += 5;
            yield return new WaitUntil(() => hp <= max);
        }
    }
    public IEnumerator MinusTensionCo(int damege)
    {
        while (Tension > zero)
        {
            yield return new WaitForSeconds(3);
            Tension -= damege;
            Debug.Log(damege);
        }
        yield break;
    }
    public IEnumerator PlusTensionCo(int tensionUp)
    {
        while (Tension < zero)
        {
            yield return new WaitForSeconds(5);
            Tension += tensionUp;
            yield return new WaitUntil(() => Hp < 100);
        }
    }
    bool CheckInLayerMask(int layerIndex)
    {
        return (monsterMask & (1 << layerIndex)) != 0;
    }
    private void Update()
    {
        playerSM.curState.Update();
        playerPos.position = this.transform.position;
        Collider[] monsterAttackZoneCol = Physics.OverlapSphere(new Vector3(transform.position.x,transform.position.y + 1, transform.position.z), 1, monsterMask);
        bool isMonsterAttackZone = monsterAttackZoneCol.Length > 0;

        if (isMonsterAttackZone)
        {
            playerSM.SetState("Caught");
            if (monsterAttackZoneCol[0].GetComponent<HiRil>() != null)
                GameManager.Instance.HirilEnding();
            else
                GameManager.Instance.HaikenEnding();
        }
        Collider[] monsterZoneCol = Physics.OverlapSphere(transform.position, monsterLookZone, monsterMask);
        bool isMonsterZone = monsterZoneCol.Length > 0;
        if (isMonsterZone)
        {
            RaycastHit hit;
            Vector3 direction = ((monsterZoneCol[0].transform.position) - transform.position).normalized;
            Debug.DrawLine(transform.position, transform.position + (direction * maxDistance), Color.blue);
            if (Physics.Raycast(transform.position, direction, out hit, maxDistance))
                IsMonsterCheck = CheckInLayerMask(hit.collider.gameObject.layer);
        }
    }
}