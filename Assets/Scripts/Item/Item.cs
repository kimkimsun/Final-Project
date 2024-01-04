using UnityEngine;
using System;
using CustomInterface;

public abstract class ItemStrategy
{
    protected Item item;
    public ItemStrategy(Item item)
    {
        this.item = item;
    }
    public abstract void Use();
}
public class CameraItem : ItemStrategy
{
    public CameraItem(Item item) : base(item)
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
    public FireCrackerItem(Item item) : base(item)
    {
        this.item = item;
    }

    public override void Use()
    {
        Debug.Log("ÆÄÁöÁöÁ÷");
    }
}
public class FlashlightItem : ItemStrategy
{
    private float batteryCharge;
    public FlashlightItem(Item item) : base(item)
    {
        this.item = item;
        this.batteryCharge = item.batteryCharge;
    }

    public override void Use()
    {
        if (batteryCharge > 0)
        {
            GameManager.Instance.player.OperationFlashLight();
        }
        else
            return;
    }
}
public enum ITEM_TYPE
{
    CAMERA,
    FIRECRACKER,
    FLASHLIGHT,
}

public class Item : MonoBehaviour, IActivable
{
    ItemStrategy itemStrategy = null;
    public ITEM_TYPE item;
    public float batteryCharge;
    protected Action action;
    void Start()
    {
        batteryCharge = 20;
        switch(item)
        {
            case ITEM_TYPE.CAMERA:
                itemStrategy = new CameraItem(this);
                    break;
            case ITEM_TYPE.FIRECRACKER:
                itemStrategy = new FireCrackerItem(this);
                break;
            case ITEM_TYPE.FLASHLIGHT:
                itemStrategy = new FlashlightItem(this);
                break;
        }
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(itemStrategy);
            itemStrategy.Use();
        }
    }
    public Action Active()
    {
        return action += () => {itemStrategy.Use();};
    }
}