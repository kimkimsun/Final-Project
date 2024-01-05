using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraItem : ItemStrategy
{
    public CameraItem(UseItem item) : base(item)
    {
        this.item = item;
    }

    public override void Use()
    {
        Debug.Log("ÂûÄ¬");
    }
}
public class FireCrackerItem : ItemStrategy
{
    public FireCrackerItem(UseItem item) : base(item)
    {
        this.item = item;
    }

    public override void Use()
    {
        Debug.Log("ÆÄÁöÁöÁ÷");
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
}
