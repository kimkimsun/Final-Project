using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class EquipItemStrategy: ItemStrategy
{
    protected EquipmentItem equipmentItem;
    public EquipItemStrategy(EquipmentItem equipmentItem)
    {
        this.equipmentItem = equipmentItem;
    }
}


public class AdrenalineItemStrategy : EquipItemStrategy
{
    EquipmentItem equip;
    public AdrenalineItemStrategy(EquipmentItem equipmentItem) :base(equipmentItem)
    {
        this.equip = equipmentItem;
        Init();
    }
    public override void Init()
    {
        explanationText = "Your stamina wears down \n a bit more and goes faster";
        equip.ExplanationText = this.explanationText;
    }

    public override void Use()
    {
        Debug.Log("각성해버려따");
    }
    public override void Exit()
    {
        Debug.Log("아드레날린 나가요");
    }
}
public class RainBootsItemStrategy : EquipItemStrategy
{
    EquipmentItem equip;
    public RainBootsItemStrategy(EquipmentItem equipmentItem):base(equipmentItem)
    {
        this.equip = equipmentItem;
        Init();
    }

    public override void Init()
    {
        explanationText = "Wearing these boots  \n will reduce the sound  \n of your footsteps";
        equip.ExplanationText = this.explanationText;
    }
    public override void Use()
    {
        Debug.Log("터벅터벅 소리 감소");
    }
    public override void Exit()
    {
        Debug.Log("장화 나가요");
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
    public override void Exit()
    {
        itemStrategy.Exit();
    }
    public override void Active()
    {
        Inventory playerInven = GameManager.Instance.player.EquipInventory;
        playerInven.AddItem(this);
    }
}