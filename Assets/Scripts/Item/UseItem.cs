using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class CameraItemtrategy : ItemStrategy
{
    public StunLight stunLight;
    public CameraItemtrategy(UseItem useItem)
    {
        this.useItem = useItem;
        stunLight = useItem.GetComponentInChildren<StunLight>();

    }

    public override void Use()
    {
        stunLight.Stun();
    }

}
public class FireCrackerItemtrategy : ItemStrategy
{
    Vector3 screenCenter;
    Rigidbody itemRB;
    SphereCollider itemCollider;

    int time = 5;
    public FireCrackerItemtrategy(UseItem useItem) 
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

public class MirrorItemtrategy : ItemStrategy
{
    public StunLight stunLight;
    public MirrorItemtrategy(UseItem useItem)
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

public class HpBuffItemtrategy : ItemStrategy
{
    int hpBuff = 5;

    public HpBuffItemtrategy(UseItem useItem)
    {
        this.useItem = useItem;
    }
    public override void Use()
    {
        GameManager.Instance.mainPlayer.Hp += hpBuff;
        GameObject.Destroy(useItem.gameObject);
    }
}

public class StaminaBuffItemtrategy : ItemStrategy
{
    int staminaBuff = 5;
    public StaminaBuffItemtrategy(UseItem useItem)
    {
        this.useItem = useItem;
    }
    public override void Use()
    {
        GameManager.Instance.mainPlayer.Stamina += staminaBuff;
        GameObject.Destroy(useItem.gameObject);
    }
}

public class SaveItemtrategy : ItemStrategy
{
    public override void Use()
    {
        throw new System.NotImplementedException();
    }
}

public class KeyItemtrategy : ItemStrategy
{
    public override void Use()
    {
        throw new System.NotImplementedException();
    }
}

public class AttackItemtrategy : ItemStrategy
{
    public override void Use()
    {
        throw new System.NotImplementedException();
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
                itemStrategy = new CameraItemtrategy(this);
               break;
           case USEITEM_TYPE.FIRECRACKER:
                itemStrategy = new FireCrackerItemtrategy(this);
               break;
            case USEITEM_TYPE.MIRROR:
                itemStrategy = new MirrorItemtrategy(this);
                break;
            case USEITEM_TYPE.HPBUFF:
                itemStrategy = new HpBuffItemtrategy(this);
                break;
            case USEITEM_TYPE.STAMINABUFF:
                itemStrategy = new StaminaBuffItemtrategy(this);
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
