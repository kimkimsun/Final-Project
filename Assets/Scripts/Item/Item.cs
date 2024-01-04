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

/*public class FlashlightItem : ItemStrategy
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
}*/

public class Item : MonoBehaviour, IActivable
{
    ItemStrategy itemStrategy = null;
    public float batteryCharge;
    protected Action action;

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