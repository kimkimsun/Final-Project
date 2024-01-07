using System.Collections;
using System.Collections.Generic;
using UnityEngine;




//public class FlashlightItem : ItemStrategy
//{

//    private float batteryCharge;
//    public FlashlightItem(EquipmentItem equipmentItem) 
//    {
//        this.batteryCharge = equipmentItem.batteryCharge;
//    }

//    public override void Use()
//    {
//        if (batteryCharge > 0)
//        {
//            /*GameManager.Instance.mainPlayer.OperationFlashLight();*/
//        }
//        else
//            return;
//    }
//}

//public class EquipmentItem : Item
//{
//    public EQUIPITEM_TYPE equipItem_Type;

//    public float batteryCharge;
//    private void Start()
//    {
//        switch (equipItem_Type)
//        {
//            case EQUIPITEM_TYPE.FLASHLIGHT:
//                itemStrategy = new FlashlightItem(this);
//                break;
//        }
//    }
//}
public class AdrenalineStrategy : ItemStrategy
{
    EquipmentItem equip;
    public AdrenalineStrategy(EquipmentItem equip)
    {
        this.equip = equip;
    }
    public override void Use()
    {
        Debug.Log("각성해버려따");
    }
}
public class RainBootsStrategy : ItemStrategy
{
    EquipmentItem equip;
    public RainBootsStrategy(EquipmentItem equip)
    {
        this.equip = equip;
    }
    public override void Use()
    {
        Debug.Log("터벅터벅 소리 감소");
    }
}
public enum EQUIPITEM_TYPE
{
    RAINBOOTS,
    ADRENALINE
}
public class EquipmentItem : Item
{
    public EQUIPITEM_TYPE equipItem_Type;
    private void Start()
    {
        switch (equipItem_Type)
        {
            case EQUIPITEM_TYPE.RAINBOOTS:
                itemStrategy = new RainBootsStrategy(this);
                break;
            case EQUIPITEM_TYPE.ADRENALINE:
                itemStrategy = new AdrenalineStrategy(this);
                break;
        }
    }
}