using System.Collections;
using UnityEngine;
using static UnityEditor.Progress;

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
    public CameraItemStrategy(UseItem useItem):base(useItem) 
    {
        stunLight = useItem.GetComponentInChildren<StunLight>();
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

    int time = 5;
    public FireCrackerItemStrategy(UseItem useItem): base(useItem)
    {
        Init();
    }

    public override void Init()
    {
        screenCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
        itemRB = useItem.GetComponentInChildren<Rigidbody>();
        itemCollider = useItem.GetComponentInChildren<SphereCollider>();
    }

    public override void Use()
    {
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
        //폭죽 사운드
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
    public MirrorItemStrategy(UseItem useItem): base(useItem)
    {
        Init();
    }

    public override void Init()
    {
        stunLight = useItem.GetComponentInChildren<StunLight>();
    }
    public override void Use()
    {
        stunLight.Stun();
        GameManager.Instance.mainPlayer.gameObject.transform.position = useItem.SponPoint.gameObject.transform.position;
    }


}

public class HpBuffItemStrategy : UseItemStrategy
{
    int hpBuff = 5;

    public HpBuffItemStrategy(UseItem useItem): base(useItem) { }

    public override void Use()
    {
        GameManager.Instance.mainPlayer.Hp += hpBuff;
        GameObject.Destroy(useItem.gameObject);
    }
}

public class StaminaBuffItemStrategy : UseItemStrategy
{
    int staminaBuff = 5;
    public StaminaBuffItemStrategy(UseItem useItem): base(useItem) { }

    public override void Use()
    {
        GameManager.Instance.mainPlayer.Stamina += staminaBuff;
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
    public KeyItemStrategy(UseItem useItem) : base(useItem) { }
    public override void Use()
    {
        throw new System.NotImplementedException();
    }
}

public class AttackItemStrategy : UseItemStrategy
{
    public AttackItemStrategy(UseItem useItem) : base(useItem) { }
    public override void Use()
    {
        throw new System.NotImplementedException();
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
            Debug.Log("마이너스배터리");
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
            Debug.Log("플러스배터리");
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
        QuickSlot quickSlot = GameManager.Instance.mainPlayer.QuickSlot;
        quickSlot.setItem(this);
        gameObject.SetActive(false);
        transform.SetParent(quickSlot.transform);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Use();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            itemStrategy.Exit();
        }
    }


}
