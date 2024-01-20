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

    #region 변수
    [Header("�κ��丮")]
    public EquipItemInventory equipInven;
    public UseItemInventory quickSlot;
    public Inventory portableInven;
    
    public InteractionAim aim;
    public FirstPersonController playerMove;    

    private LayerMask monsterMask;
    public GameEvent finalEvent;
    public GameObject itemBox;
    public GameObject hairPinSlot;

    [SerializeField] private int hp;
    [SerializeField] private float stamina;
    [SerializeField] private int tension;

    private StateMachine<Player> playerSM;
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
    #endregion
    #region 프로퍼티
    public StateMachine<Player> PlayerSM
    {
        get => playerSM; 
        set => playerSM = value;
    }
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
            //if(tension >= max)
            //{
            //    tension = max;
            //    UIManager.Instance.tensionAni.SetBool("IsDownTension", false);
            //}
            //if (tension <= 60 && playerSM.curState is not MoribundState)
            //{
            //    playerSM.SetState("Exhaustion");
            //    UIManager.Instance.tensionAni.SetBool("VeryDownTension", false);
            //    UIManager.Instance.tensionAni.SetBool("IsDownTension", true);
            //}
            //else if (tension > 60 && playerSM.curState is not MoribundState)
            //{
            //    UIManager.Instance.tensionAni.SetBool("IsDownTension", false);
            //    UIManager.Instance.tensionAni.SetBool("VeryDownTension", true);
            //    playerSM.SetState("IdleState");
            //}
        }
    }
    public int Hp
    {
        get { return hp; }
        set
        {
            hp = value;
            //if (hp <= zero)
            //{
            //    hp = zero;
            //    ScenesManager.Instance.DieScene();
            //}
            //else if (hp <= 30)
            //{
            //    Debug.Log(hp);
            //    playerSM.SetState("Moribund");
            //    UIManager.Instance.hpAni.SetBool("IsDownHp", false);
            //    UIManager.Instance.hpAni.SetBool("IsVeryDown", true);
            //}
            //else if (hp <= 50)
            //{
            //    UIManager.Instance.hpAni.SetBool("IsVeryDown", false);
            //    UIManager.Instance.hpAni.SetBool("IsDownHp", true);
            //    Debug.Log(hp);
            //}
            //else if (hp >= max)
            //{
            //    hp = max;
            //    UIManager.Instance.hpAni.SetBool("IsDownHp", false);
            //}
        }
    }
    #endregion

    private void Start()
    {
        playerMove = GetComponent<FirstPersonController>();
        playerSM = new StateMachine<Player>();
        playerSM.owner = this;

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
        finalEvent.RegisterListener(() => { this.enabled = true; });
    }
    public IEnumerator MinusTensionCo(int damege)
    {
        while (Tension > zero)
        {
            yield return new WaitForSeconds(3);
            Tension -= damege;
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), 1);
    }
    private void Update()
    {

        playerSM.curState.Update();
        Collider[] monsterAttackZoneCol = Physics.OverlapSphere(new Vector3(transform.position.x,transform.position.y + 1, transform.position.z), 1, monsterMask);
        bool isMonsterAttackZone = monsterAttackZoneCol.Length > 0;
        if (isMonsterAttackZone && PlayerSM.curState is not CaughtState)
            playerSM.SetState("Caught");
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