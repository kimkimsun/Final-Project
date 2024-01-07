using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class CameraItem : ItemStrategy
{
    public StunLight stunLight;
    public CameraItem(UseItem useItem)
    {
        this.useItem = useItem;
        stunLight = useItem.GetComponentInChildren<StunLight>();

    }

    public override void Use()
    {
        stunLight.Stun();
    }

}
public class FireCrackerItem : ItemStrategy
{
    Vector3 screenCenter;
    Rigidbody itemRB;
    SphereCollider itemCollider;

    int time = 3;
    public FireCrackerItem(UseItem useItem) 
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
        if (Physics.Raycast(ray, out hit, 10))
        {
            Vector3 nextVec = hit.point - useItem.transform.position;
            nextVec.y = 5;
            itemRB.isKinematic = false;
            itemCollider.isTrigger = false;

            itemRB.AddForce(nextVec ,ForceMode.Impulse);
            itemRB.AddTorque(Vector3.left *10 , ForceMode.Impulse);
        
        }
        //ÆøÁ× »ç¿îµå
        useItem.StartCoroutine(AttractionCo());
    }

    IEnumerator AttractionCo()
    {
        itemCollider.enabled = true;
        yield return new WaitForSeconds(time);
        itemCollider.enabled = false;

    }
}
public enum USEITEM_TYPE
{
    CAMERA,
    FIRECRACKER
}

public class UseItem : Item
{
    public USEITEM_TYPE useItem_Type;
    
    private void Start()
    {

       switch (useItem_Type)
       {
           case USEITEM_TYPE.CAMERA:
                itemStrategy = new CameraItem(this);
               break;
           case USEITEM_TYPE.FIRECRACKER:
                itemStrategy = new FireCrackerItem(this);
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
