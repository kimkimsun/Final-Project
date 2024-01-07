using System.Collections;
using UnityEngine;


public class CameraItemStrategy : ItemStrategy
{
    public StunLight stunLight;
    public CameraItemStrategy(UseItem useItem)
    {
        this.useItem = useItem;
        stunLight = useItem.GetComponentInChildren<StunLight>();

    }

    public override void Use()
    {
        stunLight.Stun();
    }

}
public class FireCrackerItemStrategy : ItemStrategy
{
    Vector3 screenCenter;
    Rigidbody itemRB;
    SphereCollider itemCollider;

    int time = 5;
    public FireCrackerItemStrategy(UseItem useItem) 
    {
        this.useItem = useItem;
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
        //ÆøÁ× »ç¿îµå
        useItem.StartCoroutine(AttractionCo());
    }

    IEnumerator AttractionCo()
    {
        itemCollider.enabled = true;
        yield return new WaitForSeconds(time);
        GameObject.Destroy(useItem.gameObject);

    }
}

public class MirrorItemStrategy : ItemStrategy
{
    public StunLight stunLight;
    public MirrorItemStrategy(UseItem useItem)
    {
        this.useItem = useItem;
        stunLight = useItem.GetComponentInChildren<StunLight>();
    }
    public override void Use()
    {
        stunLight.Stun();
        GameManager.Instance.mainPlayer.gameObject.transform.position = useItem.SponPoint.gameObject.transform.position;
    }


}

public class HpBuffItemStrategy : ItemStrategy
{
    int hpBuff = 5;

    public HpBuffItemStrategy(UseItem useItem)
    {
        this.useItem = useItem;
    }
    public override void Use()
    {
        GameManager.Instance.mainPlayer.Hp += hpBuff;
        GameObject.Destroy(useItem.gameObject);
    }
}

public class StaminaBuffItemStrategy : ItemStrategy
{
    int staminaBuff = 5;
    public StaminaBuffItemStrategy(UseItem useItem)
    {
        this.useItem = useItem;
    }
    public override void Use()
    {
        GameManager.Instance.mainPlayer.Stamina += staminaBuff;
        GameObject.Destroy(useItem.gameObject);
    }
}

public class SaveItemStrategy : ItemStrategy
{
    public override void Use()
    {
        throw new System.NotImplementedException();
    }
}

public class KeyItemStrategy : ItemStrategy
{
    public override void Use()
    {
        throw new System.NotImplementedException();
    }
}

public class AttackItemStrategy : ItemStrategy
{
    public override void Use()
    {
        throw new System.NotImplementedException();
    }
}


public class FlashlightItemStrategy : ItemStrategy
{

    Light flashlight;

    IEnumerator minusBatteryCo;
    IEnumerator plusBatteryCo;

    int minBriught; 
    int maxBriught; 
    int battery;
    int minBattery;
    int maxBattery; 

     int Battery
    {
        get { return battery; }
        set 
        {
            battery = value;
            if(battery < minBattery)
                battery = minBattery;
            if( battery > maxBattery)
                battery = maxBattery;
        }
    }

    public FlashlightItemStrategy(UseItem useItem) 
    {
        minBriught = 0;
        maxBriught = 10;
        Battery = 50;
        minBattery = 30;
        maxBattery = 50;

        minusBatteryCo = MinusBatteryCo();
        plusBatteryCo = PlusBatteryCo();

        this.useItem = useItem;
        flashlight = useItem.GetComponentInChildren<Light>();
        flashlight.intensity = minBriught;
    }

    public override void Use()
    {
        useItem.StartCoroutine(minusBatteryCo);
    }

    public override void Exit()
    {
        useItem.StartCoroutine(plusBatteryCo);
    }

    IEnumerator MinusBatteryCo()
    {        
        while (Battery > 0)
        {
            Battery -= 1;
            yield return new WaitForSeconds(1.5f);
        }
        flashlight.intensity = minBriught;
    }

    IEnumerator PlusBatteryCo()
    {
        while (Battery < maxBattery)
        {
            battery += 1;
            yield return new WaitForSeconds(2.5f);
        }
        flashlight.intensity = maxBriught;
    }
}




public enum USEITEM_TYPE
{
    CAMERA,
    FIRECRACKER,
    MIRROR,
    HPBUFF,
    STAMINABUFF
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


        }

    }
   
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            
            Active();

        }
    }


}
