using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AdrenalineItemStrategy : ItemStrategy
{
    EquipmentItem equip;
    public AdrenalineItemStrategy(EquipmentItem equip)
    {
        this.equip = equip;
    }
    public override void Use()
    {
        Debug.Log("�����ع�����");
    }
    public override void Exit()
    {
        Debug.Log("�Ƶ巹���� ������");
    }
}
public class RainBootsItemStrategy : ItemStrategy
{
    EquipmentItem equip;
    public RainBootsItemStrategy(EquipmentItem equip)
    {
        this.equip = equip;
    }
    public override void Use()
    {
        Debug.Log("�͹��͹� �Ҹ� ����");
    }
    public override void Exit()
    {
        Debug.Log("��ȭ ������");
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
                itemStrategy = new RainBootsItemStrategy(this);
                break;
            case EQUIPITEM_TYPE.ADRENALINE:
                itemStrategy = new AdrenalineItemStrategy(this);
                break;
        }
    }
    public void Exit()
    {
        itemStrategy.Exit();
    }
}