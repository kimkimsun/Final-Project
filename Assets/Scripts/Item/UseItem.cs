using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UseItemStrategy
{
    protected Item item;
    public UseItemStrategy(UseItem item)
    {
        this.item = item;
    }
    public abstract void Use();
}

public class CameraItem : UseItemStrategy
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
public class FireCrackerItem : UseItemStrategy
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
    public UseItemStrategy useItemStrategy;
    public USEITEM_TYPE useItem_Type;

    private void Start()
    {
        void Start()
        {
            switch (useItem_Type)
            {
                case USEITEM_TYPE.CAMERA:
                    useItemStrategy = new CameraItem(this);
                    break;
                case USEITEM_TYPE.FIRECRACKER:
                    useItemStrategy = new FireCrackerItem(this);
                    break;
            }
        }
    }
}
