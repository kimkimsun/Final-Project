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
    public Image batteryCharge;
    public TextMeshProUGUI hpText;
    public Light flashlight;
    public float battery = 60;

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
    private float minBright = 0;
    private float maxBright = 10;
    private float minBattery = 0;
    private float maxBattery = 60;
    private bool useing;
    private bool isRegulate;
    private bool isMonsterCheck;
    private bool isMonsterAttackCheck;
    private bool caughtSetState;
    private IEnumerator minusTensionCo;
    private IEnumerator plusTensionCo;
    private IEnumerator minusBatteryCo;
    private IEnumerator plusBatteryCo;
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
            hpText.text = hp.ToString();
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

        plusBatteryCo = PlusBatteryCo();
        minusBatteryCo = MinusBatteryCo();

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
    public void UseFlash()
    {
        useing = !useing;
        if (useing)
        {
            StopCoroutine(plusBatteryCo);
            StartCoroutine(minusBatteryCo);
        }
        else
        {
            StopFlash();
        }
    }
    public void StopFlash()
    {
        StopCoroutine(minusBatteryCo);
        StartCoroutine(plusBatteryCo);
    }
    IEnumerator MinusBatteryCo()
    {
        Debug.Log("마이너스");
        while (battery > minBattery)
        {
            flashlight.intensity = maxBright;
            yield return new WaitForSeconds(1f);
            batteryCharge.fillAmount = battery / 60f;
            battery -= 1;
            if (battery <= minBattery)
            {
                StopFlash();
            }
            yield return new WaitUntil(() => battery > minBattery);
        }
    }

    IEnumerator PlusBatteryCo()
    {
        Debug.Log("플러스");
        while (battery < maxBattery)
        {
            flashlight.intensity = minBright;
            yield return new WaitForSeconds(0.6f);
            batteryCharge.fillAmount = battery / 60f;
            battery += 1;
            yield return new WaitUntil(() => battery < maxBattery);
        }
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
        isRegulate = false;
        while (Tension < max)
        {
            yield return new WaitForSeconds(5);
            Tension += tensionUp;
            yield return new WaitUntil(() => Tension < 100);
        }
    }
    bool CheckInLayerMask(int layerIndex)
    {
        return (monsterMask & (1 << layerIndex)) != 0;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        playerSM.curState.Update();
        Collider[] monsterZoneCol = Physics.OverlapSphere(transform.position, monsterLookZone, monsterMask);
        bool isMonsterZone = monsterZoneCol.Length > 0;
        if (isMonsterZone)
        {
            RaycastHit hit;
            Vector3 direction = ((monsterZoneCol[0].transform.position) - transform.position).normalized;
            if (Physics.Raycast(transform.position, direction, out hit, maxDistance))
                isMonsterCheck = CheckInLayerMask(hit.collider.gameObject.layer);
            if (isMonsterCheck && !isRegulate)
            {
                StopCoroutine(plusTensionCo);
                StartCoroutine(minusTensionCo);
                isRegulate = true;
            }
            else if (!isMonsterCheck && isRegulate)
            {
                StopCoroutine(minusTensionCo);
                StartCoroutine(plusTensionCo);
                isRegulate = false;
            }
        }
        else if(!isMonsterZone)
        {
            StopCoroutine(minusTensionCo);
            StartCoroutine(plusTensionCo);
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