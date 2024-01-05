using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EQUIPITEM_TYPE
{
    FLASHLIGHT
}


public class FlashlightItem : ItemStrategy
{

    private float batteryCharge;
    public FlashlightItem(EquipmentItem item) : base(item)
    {
        this.batteryCharge = item.batteryCharge;
    }

    public override void Use()
    {
        if (batteryCharge > 0)
        {
            /*GameManager.Instance.mainPlayer.OperationFlashLight();*/
        }
        else
            return;
    }
}

public class EquipmentItem : Item
{
    public EQUIPITEM_TYPE equipItem_Type;

    public float batteryCharge;
    private void Start()
    {
        

        switch (equipItem_Type)
        {
            case EQUIPITEM_TYPE.FLASHLIGHT:
                itemStrategy = new FlashlightItem(this);
                break;
        }

    }
}
