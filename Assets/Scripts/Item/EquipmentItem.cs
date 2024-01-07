using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AdrenalineStrategy : ItemStrategy
{
    EquipmentItem equip;
    public AdrenalineStrategy(EquipmentItem equip)
    {
        this.equip = equip;
    }
    public override void Use()
    {
        Debug.Log("�����ع�����");
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
        Debug.Log("�͹��͹� �Ҹ� ����");
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