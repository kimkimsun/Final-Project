using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class CameraItem : ItemStrategy
{
    float radius = 5;

    public CameraItem(UseItem useItem)
    {
        this.useItem = useItem;

    }

    public override void Use()
    {
        
        Debug.Log("카메라");
    }

}
public class FireCrackerItem : ItemStrategy
{
    public FireCrackerItem(UseItem useItem) 
    {
        this.useItem = useItem;
    }

    public override void Use()
    {
        Debug.Log("파지지직");
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
    public GameObject flashLight;
    


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
