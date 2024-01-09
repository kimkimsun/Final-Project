using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
        useItem.firstDic.Add(this, isFirstCamera);
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
        useItem.firstDic.Add(this, isFirstFireCracker);
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
        if(isFirstFireCracker)
        {
            UIManager.Instance.useItemInfo.SetInfo(useItem);
            UIManager.Instance.useItemInfo.gameObject.SetActive(true);
            isFirstFireCracker = false;
        }
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);     
        
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            Vector3 nextVec = hit.point - useItem.transform.position;
            nextVec.y = 5;
            itemRB.isKinematic = false;
            itemCollider.isTrigger = false;

            itemRB.AddForce(nextVec *1.5f,ForceMode.Impulse );
            itemRB.AddTorque(Vector3.left *5 , ForceMode.Impulse);
        }
        //���� ����
        useItem.StartCoroutine(AttractionCo());
    }
    IEnumerator AttractionCo()
    {
        itemCollider.enabled = true;
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
        useItem.firstDic.Add(this, isFirstMirror);
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
        if (isFirstMirror)
        {
            UIManager.Instance.useItemInfo.SetInfo(useItem);
            UIManager.Instance.useItemInfo.gameObject.SetActive(true);
            isFirstMirror = false;
        }
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
        useItem.firstDic.Add(this, isFirstHpBuff);
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
        if (isFirstHpBuff)
        {
            UIManager.Instance.useItemInfo.SetInfo(useItem);
            UIManager.Instance.useItemInfo.gameObject.SetActive(true);
            isFirstHpBuff = false;
        }
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
        useItem.firstDic.Add(this, isFirstStaminaBuff);
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
        if (isFirstStaminaBuff)
        {
            UIManager.Instance.useItemInfo.SetInfo(useItem);
            UIManager.Instance.useItemInfo.gameObject.SetActive(true);
            isFirstStaminaBuff = false;
        }
        GameManager.Instance.player.Stamina += staminaBuff;
        GameObject.Destroy(useItem.gameObject);
    }
}
public class SaveItemStrategy : UseItemStrategy
{
    public SaveItemStrategy(UseItem useItem) : base(useItem) { }

    public override void Use()
    {
        throw new System.NotImplementedException();
    }
}

public class KeyItemStrategy : UseItemStrategy
{
    static bool isFirstKey;
    public KeyItemStrategy(UseItem useItem) : base(useItem) { Init(); }

    public override void Init()
    {
        isFirstKey = true;
        useItem.firstDic.Add(this, isFirstKey);
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
        if (isFirstKey)
        {
            UIManager.Instance.useItemInfo.SetInfo(useItem);
            UIManager.Instance.useItemInfo.gameObject.SetActive(true);
            isFirstKey = false;
        }
    }
}

public class AttackItemStrategy : UseItemStrategy
{
    static bool isFirstAttackItem;
    public AttackItemStrategy(UseItem useItem) : base(useItem) { }

    public override void Init()
    {
        isFirstAttackItem = true;
        useItem.firstDic.Add(this, isFirstAttackItem);
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
        if (isFirstAttackItem)
        {
            UIManager.Instance.useItemInfo.SetInfo(useItem);
            UIManager.Instance.useItemInfo.gameObject.SetActive(true);
            isFirstAttackItem = false;
        }
    }
}


public class FlashlightItemStrategy : UseItemStrategy
{

    Light flashlight;

    //IEnumerator minusBatteryCo;
    //IEnumerator plusBatteryCo;

    int minBright; 
    int maxBright; 
    int battery;
    int minBattery;
    int maxBattery; 

    public int Battery
    {
        get { return battery; }
        set 
        {
            battery = value;
            if(battery <= minBattery)
                battery = minBattery;
            if( battery >= maxBattery)
                battery = maxBattery;
        }
    }

    public FlashlightItemStrategy(UseItem useItem) : base(useItem)
    {
        Init();
    }

    public override void Init()
    {
        minBright = 0;
        maxBright = 10;
        minBattery = 0;
        maxBattery = 50;
        Battery = 50;
        //minusBatteryCo = MinusBatteryCo();
        //plusBatteryCo = PlusBatteryCo();

        flashlight = useItem.GetComponentInChildren<Light>();
        flashlight.intensity = minBright;
    }

    public override void Use()
    {
        //useItem.StopAllCoroutines();
        useItem.StartCoroutine(MinusBatteryCo());
    }

    public override void Exit()
    {
        //useItem.StopAllCoroutines();
        useItem.StartCoroutine(PlusBatteryCo());
    }

    IEnumerator MinusBatteryCo()
    {
        while (true)
        {
            if (Battery == 0)
            {
                flashlight.intensity = minBright;
                Exit();
            }
            flashlight.intensity = maxBright;
            Battery -= 10;
            Debug.Log("���̳ʽ����͸�");
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator PlusBatteryCo()
    {
        while (true)
        {
            if (Battery == maxBattery)
                continue;
            battery += 10;
            Debug.Log("�÷������͸�");
            yield return new WaitForSeconds(2.5f);
        }
    }
}
public enum USEITEM_TYPE
{
    CAMERA,
    FIRECRACKER,
    MIRROR,
    HPBUFF,
    STAMINABUFF,
    FLASHLIGHT
}

public class UseItem : Item
{
    public UseItemStrategy UseItemstrategy;
    public USEITEM_TYPE useItem_Type;
    public GameObject SponPoint;
    public Dictionary<ItemStrategy,bool> firstDic = new Dictionary<ItemStrategy,bool>();
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
            case USEITEM_TYPE.FLASHLIGHT:
                itemStrategy = new FlashlightItemStrategy(this);
                break;
        }

    }


    public override void Active()
    {
        ((UseItemStrategy)itemStrategy).PrintInfo();
        Inventory quickSlot = GameManager.Instance.player.QuickSlot;
        quickSlot.AddItem(this);
        gameObject.SetActive(false);
        transform.SetParent(quickSlot.transform);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            itemStrategy.Exit();
        }
    }
}
