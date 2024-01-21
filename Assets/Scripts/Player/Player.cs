using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using Unity.VisualScripting;
using CustomInterface;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using System;
using TMPro;

[Serializable]
public class Player : MonoBehaviour
{
    [SerializeField] private static int exitItemCount;

    #region 변수
    [Header("�κ��丮")]
    public EquipItemInventory equipInven;
    public UseItemInventory quickSlot;
    public InteractionAim aim;
    public FirstPersonController playerMove;    
    public GameEvent finalEvent;
    public GameObject itemBox;
    public GameObject hairPinSlot;
    public Image stminaImage;
    public TextMeshProUGUI hpText;

    [SerializeField] private int hp;
    [SerializeField] private float stamina;
    [SerializeField] private int tension;

    private LayerMask monsterMask;
    private StateMachine<Player> playerSM;
    private int tensionDwon = 5;
    private int tensionUp = 3;
    private int maxDistance;
    private int max = 100;
    private int zero = 0;
    private int finalKey = 5;
    private int monsterLookZone;
    private bool isRegulate;
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
                isRegulate = true;
                if (isRegulate)
                {
                    StopCoroutine(plusTensionCo);
                    StartCoroutine(minusTensionCo);
                }
                isRegulate = false;
            }
            else
            {
                isRegulate = true;
                StopCoroutine(minusTensionCo);
                StartCoroutine(plusTensionCo);
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
            stminaImage.fillAmount = stamina/100;
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
            if (tension >= max)
            {
                tension = max;
                UIManager.Instance.tensionAni.SetBool("IsDownTension", false);
            }
            if (tension <= 60 && playerSM.curState is not MoribundState && playerSM.curState is not ExhaustionState)
            {
                playerSM.SetState("Exhaustion");
                UIManager.Instance.tensionAni.SetBool("VeryDownTension", false);
                UIManager.Instance.tensionAni.SetBool("IsDownTension", true);
            }
            else if (tension > 60 && playerSM.curState is not MoribundState && playerSM.curState is not IdleState)
            {
                UIManager.Instance.tensionAni.SetBool("IsDownTension", false);
                UIManager.Instance.tensionAni.SetBool("VeryDownTension", true);
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
            hpText.text = "Hp " + hp.ToString();
            if (hp <= zero)
            {
                hp = zero;
                ScenesManager.Instance.DieScene();
            }
            else if (hp <= 30 && playerSM.curState is not MoribundState)
            {
                playerSM.SetState("Moribund");
                UIManager.Instance.hpAni.SetBool("IsDownHp", false);
                UIManager.Instance.hpAni.SetBool("IsVeryDown", true);
            }
            else if (hp <= 50)
            {
                UIManager.Instance.hpAni.SetBool("IsVeryDown", false);
                UIManager.Instance.hpAni.SetBool("IsDownHp", true);
            }
            else if (hp >= max)
            {
                hp = max;
                UIManager.Instance.hpAni.SetBool("IsDownHp", false);
            }
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
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log(tension);
        }
        playerSM.curState.Update();
        Collider[] monsterZoneCol = Physics.OverlapSphere(transform.position, monsterLookZone, monsterMask);
        bool isMonsterZone = monsterZoneCol.Length > 0;
        if (isMonsterZone)
        {
            RaycastHit hit;
            Vector3 direction = ((monsterZoneCol[0].transform.position) - transform.position).normalized;
            if (Physics.Raycast(transform.position, direction, out hit, maxDistance))
                IsMonsterCheck = CheckInLayerMask(hit.collider.gameObject.layer);
            Debug.Log("SDASD" + IsMonsterCheck);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Monster>() != null)
        {
            if (PlayerSM.curState is not CaughtState)
                playerSM.SetState("Caught");
        }
    }
}