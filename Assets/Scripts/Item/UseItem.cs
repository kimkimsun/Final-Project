using CustomInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class UseItemStrategy: ItemStrategy
{
    protected UseItem useItem;   
    public UseItemStrategy(UseItem useItem)
    {
        this.useItem = useItem;
    }

    public virtual void PrintInfo() 
    {
        UIManager.Instance.useItemInfo.SetInfo(useItem);
        UIManager.Instance.useItemInfo.gameObject.SetActive(true);
    }

    public override void Use()
    {
        useItem.transform.SetParent(null);
    }
}

public class CameraItemStrategy : UseItemStrategy
{
    public StunLight stunLight;
    static bool isFirstCamera;
    public CameraItemStrategy(UseItem useItem):base(useItem) 
    {
        Init();
    }

    public override void Init()
    {
        stunLight = useItem.GetComponentInChildren<StunLight>();
        isFirstCamera = true;
    }

    public override void PrintInfo() 
    { 
        if (isFirstCamera) 
        { 
            base.PrintInfo();
        }
        isFirstCamera = false;
    }
    public override void Use()
    {
        base.Use();
        stunLight.enabled = true;   
        stunLight.Stun();
    }
}
public class FireCrackerItemStrategy : UseItemStrategy
{
    Vector3 screenCenter;
    Rigidbody itemRB;
    SphereCollider itemCollider;

    static bool isFirstFireCracker;
    int time = 5;
    public FireCrackerItemStrategy(UseItem useItem) : base(useItem)
    {
        Init();
    }

    public override void Init()
    {
        isFirstFireCracker = true;
        screenCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
        itemRB = useItem.GetComponentInChildren<Rigidbody>();
        itemCollider = useItem.GetComponentInChildren<SphereCollider>();
    }

    public override void PrintInfo()
    {
        if (isFirstFireCracker)
        {
            base.PrintInfo();
        }
        isFirstFireCracker = false;
    }

    public override void Use()
    {
        base.Use();
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        itemRB.isKinematic = false;
        itemCollider.isTrigger = false;

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            Vector3 nextVec = hit.point - useItem.transform.position;
            Vector3 nextVeDirection = nextVec.normalized;
            nextVeDirection.y = 2;

            itemRB.AddForce(nextVeDirection * 3, ForceMode.Impulse ); 
            itemRB.AddTorque(Vector3.left *5 , ForceMode.Impulse);
            
        }
        useItem.StartCoroutine(AttractionCo());
    }
    IEnumerator AttractionCo()
    {
        itemCollider.enabled = true;
        itemCollider.isTrigger = true;
        itemCollider.radius = 3;
        yield return new WaitForSeconds(time);
        GameObject.Destroy(useItem.gameObject);

    }
}

public class MirrorItemStrategy : UseItemStrategy
{
    public StunLight stunLight;
    static bool isFirstMirror;
    public MirrorItemStrategy(UseItem useItem): base(useItem)
    {
        Init();
    }

    public override void Init()
    {
        stunLight = useItem.GetComponentInChildren<StunLight>();
        isFirstMirror = true;
    }
    public override void PrintInfo()
    {
        if (isFirstMirror)
        {
            base.PrintInfo();
        }
        isFirstMirror = false;
    }

    public override void Use()
    {
        base.Use();
        stunLight.Stun();
        GameManager.Instance.player.gameObject.transform.position = useItem.SponPoint.gameObject.transform.position;
    }
}

public class HpBuffItemStrategy : UseItemStrategy
{
    int hpBuff = 5;
    static bool isFirstHpBuff;

    public HpBuffItemStrategy(UseItem useItem): base(useItem) { Init(); }

    public override void Init()
    {
        isFirstHpBuff = true;
    }

    public override void PrintInfo()
    {
        if (isFirstHpBuff)
        {
            base.PrintInfo();
        }
        isFirstHpBuff = false;
    }
    public override void Use()
    {
        base.Use();
        GameManager.Instance.player.Hp += hpBuff;
        GameObject.Destroy(useItem.gameObject);
    }
}

public class StaminaBuffItemStrategy : UseItemStrategy
{
    int staminaBuff = 5;
    static bool isFirstStaminaBuff;
    public StaminaBuffItemStrategy(UseItem useItem): base(useItem) { Init(); }

    public override void Init()
    {
        isFirstStaminaBuff = true;
    }
    public override void PrintInfo()
    {
        if (isFirstStaminaBuff)
        {
            base.PrintInfo();
        }
        isFirstStaminaBuff = false;
    }

    public override void Use()
    {
        base.Use();
        GameManager.Instance.player.Stamina += staminaBuff;
        GameObject.Destroy(useItem.gameObject);
    }
}

public class KeyItemStrategy : UseItemStrategy
{
    static bool isFirstKey;
    public KeyItemStrategy(UseItem useItem) : base(useItem) { Init(); }

    public override void Init()
    {
        isFirstKey = true;
    }
    public override void PrintInfo()
    {
        if (isFirstKey)
        {
            base.PrintInfo();
        }
        isFirstKey = false;
    }
    public override void Use()
    {
        base.Use();
        if (isFirstKey)
        {
            UIManager.Instance.useItemInfo.SetInfo(useItem);
            UIManager.Instance.useItemInfo.gameObject.SetActive(true);
            isFirstKey = false;
        }
    }
}

public class HairPinItemStrategy : UseItemStrategy
{
    static bool isFirstAttackItem;
    Rigidbody monsterRb;
    Monster monster;
    Player player = GameManager.Instance.player;
    public HairPinItemStrategy(UseItem useItem) : base(useItem) { }

    public override void Init()
    {
        isFirstAttackItem = true;
    }

    public override void PrintInfo()
    {
        if (isFirstAttackItem)
        {
            base.PrintInfo();
        }
        isFirstAttackItem = false;
    }

    public override void Use()
    {
        useItem.transform.SetParent(player.hairPinSlot.transform);
        useItem.transform.position = player.hairPinSlot.transform.position;
        player.playerMove.PlayerAni.SetTrigger("isAttack");
    }

    protected IEnumerator CaughtCo()
    {
        while (useItem.escapeCircle.fillAmount < 1)
        {
            yield return null;
            if (useItem.escapeCircle.fillAmount < 0.6f)
                useItem.escapeCircle.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            else if (useItem.escapeCircle.fillAmount > 0.6f)
                useItem.escapeCircle.GetComponent<Image>().color = new Color(0, 1, 0, 1);
            useItem.escapeCircle.gameObject.SetActive(true);
            useItem.escapeCircle.fillAmount += (Time.deltaTime / 2);
        }
        Debug.Log("죽음");
    }
}
public class ExitItemStrategy : UseItemStrategy
{
    static bool isExitItme;
    public ExitItemStrategy(UseItem useItem) : base(useItem) { }

    public override void Init()
    {
        isExitItme = true;
    }

    public override void PrintInfo()
    {
        if (isExitItme)
        {
            base.PrintInfo();
        }
        isExitItme = false;
    }
    public override void Use()
    {
        base.Use();
        if (GameManager.Instance.player.ExitItemCount <= 5)
        {
            //어떤 이벤트 실행
            useItem.transform.SetParent(null);
        }
        else
            return;
    }
}


public enum USEITEM_TYPE
{
    CAMERA,
    FIRECRACKER,
    MIRROR,
    HPBUFF,
    STAMINABUFF,
    FLASHLIGHT,
    HAIRPIN,
    KEY,
    EXIT
}
[Serializable]
public class UseItem : Item
{
    public UseItemStrategy UseItemstrategy;
    public USEITEM_TYPE useItem_Type;
    public GameObject SponPoint;
    public Image escapeCircle;
    private void Start()
    {
       switch (useItem_Type)
       {
           case USEITEM_TYPE.CAMERA:
                itemStrategy = new CameraItemStrategy(this);
               break;
           case USEITEM_TYPE.FIRECRACKER:
                itemStrategy = new FireCrackerItemStrategy(this);
               break;
            case USEITEM_TYPE.MIRROR:
                itemStrategy = new MirrorItemStrategy(this);
                break;
            case USEITEM_TYPE.HPBUFF:
                itemStrategy = new HpBuffItemStrategy(this);
                break;
            case USEITEM_TYPE.STAMINABUFF:
                itemStrategy = new StaminaBuffItemStrategy(this);
                break;
            case USEITEM_TYPE.HAIRPIN:
                itemStrategy = new HairPinItemStrategy(this);
                break;
            case USEITEM_TYPE.KEY:
                itemStrategy = new KeyItemStrategy(this);
                break;
            case USEITEM_TYPE.EXIT:
                itemStrategy = new ExitItemStrategy(this);
                break;
        }

    }

    public override void Active()
    {
        ((UseItemStrategy)itemStrategy).PrintInfo();
        Inventory quickSlot = GameManager.Instance.player.QuickSlot;
        GameObject itemBox = GameManager.Instance.player.itemBox;
        quickSlot.AddItem(this);
        gameObject.SetActive(false);
        transform.SetParent(itemBox.transform);
    }
}
