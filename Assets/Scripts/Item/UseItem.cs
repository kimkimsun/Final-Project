using System.Collections;
using UnityEngine;

public abstract class UseItemStrategy: ItemStrategy
{
    protected UseItem useItem;   
    public UseItemStrategy(UseItem useItem)
    {
        this.useItem = useItem;
    }
}

public class CameraItemStrategy : UseItemStrategy
{
    public StunLight stunLight;
    static bool isCamera;
    public CameraItemStrategy(UseItem useItem):base(useItem) 
    {
        Init();
    }

    public override void Init()
    {
        stunLight = useItem.GetComponentInChildren<StunLight>();
        isCamera = true;
    }
    public override void Use()
    {
        if(isCamera)
        {
            UIManager.Instance.useItemInfo.SetInfo(useItem);
            UIManager.Instance.useItemInfo.gameObject.SetActive(true);
            isCamera = false;
        }
        stunLight.Stun();
    }
}
public class FireCrackerItemStrategy : UseItemStrategy
{
    Vector3 screenCenter;
    Rigidbody itemRB;
    SphereCollider itemCollider;

    static bool isFireCracker;
    int time = 5;
    public FireCrackerItemStrategy(UseItem useItem): base(useItem)
    {
        Init();
    }
    public override void Init()
    {
        isFireCracker = true;
        screenCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
        itemRB = useItem.GetComponentInChildren<Rigidbody>();
        itemCollider = useItem.GetComponentInChildren<SphereCollider>();
    }

    public override void Use()
    {
        if(isFireCracker)
        {
            UIManager.Instance.useItemInfo.SetInfo(useItem);
            UIManager.Instance.useItemInfo.gameObject.SetActive(true);
            isFireCracker = false;
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
    static bool isMirror;
    public MirrorItemStrategy(UseItem useItem): base(useItem)
    {
        Init();
    }

    public override void Init()
    {
        stunLight = useItem.GetComponentInChildren<StunLight>();
        isMirror = true;
    }
    public override void Use()
    {
        if (isMirror)
        {
            UIManager.Instance.useItemInfo.SetInfo(useItem);
            UIManager.Instance.useItemInfo.gameObject.SetActive(true);
            isMirror = false;
        }
        stunLight.Stun();
        GameManager.Instance.player.gameObject.transform.position = useItem.SponPoint.gameObject.transform.position;
    }
}

public class HpBuffItemStrategy : UseItemStrategy
{
    int hpBuff = 5;
    static bool isHpBuff;

    public HpBuffItemStrategy(UseItem useItem): base(useItem) { Init(); }

    public override void Init()
    {
        isHpBuff = true;
    }
    public override void Use()
    {
        if (isHpBuff)
        {
            UIManager.Instance.useItemInfo.SetInfo(useItem);
            UIManager.Instance.useItemInfo.gameObject.SetActive(true);
            isHpBuff = false;
        }
        GameManager.Instance.player.Hp += hpBuff;
        GameObject.Destroy(useItem.gameObject);
    }
}

public class StaminaBuffItemStrategy : UseItemStrategy
{
    int staminaBuff = 5;
    static bool isStaminaBuff;
    public StaminaBuffItemStrategy(UseItem useItem): base(useItem) { Init(); }

    public override void Init()
    {
        isStaminaBuff = true;
    }
    public override void Use()
    {
        if (isStaminaBuff)
        {
            UIManager.Instance.useItemInfo.SetInfo(useItem);
            UIManager.Instance.useItemInfo.gameObject.SetActive(true);
            isStaminaBuff = false;
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
    static bool isKey;
    public KeyItemStrategy(UseItem useItem) : base(useItem) { Init(); }

    public override void Init()
    {
        isKey = true;
    }

    public override void Use()
    {
        if (isKey)
        {
            UIManager.Instance.useItemInfo.SetInfo(useItem);
            UIManager.Instance.useItemInfo.gameObject.SetActive(true);
            isKey = false;
        }
    }
}

public class AttackItemStrategy : UseItemStrategy
{
    static bool isAttackItem;
    public AttackItemStrategy(UseItem useItem) : base(useItem) { }

    public override void Init()
    {
        isAttackItem = true;
    }
    public override void Use()
    {
        if (isAttackItem)
        {
            UIManager.Instance.useItemInfo.SetInfo(useItem);
            UIManager.Instance.useItemInfo.gameObject.SetActive(true);
            isAttackItem = false;
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
    public USEITEM_TYPE useItem_Type;
    public GameObject SponPoint;
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
        QuickSlot quickSlot = GameManager.Instance.player.QuickSlot;
        quickSlot.setItem(this);
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
