using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public abstract class EquipItemStrategy : ItemStrategy
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
    public AdrenalineItemStrategy(EquipmentItem equipmentItem) : base(equipmentItem)
    {
        this.equip = equipmentItem;
        Init();
    }
    public override void Init()
    {
        explanationText = "���׹̳ʰ� �� ������ �ް� �� ������ ���ϴ�.";
        equip.ExplanationText = this.explanationText;
    }

    public override void Use()
    {
        GameManager.Instance.player.playerMove.PlusStamina = 12;
        GameManager.Instance.player.playerMove.MinusStamina = 3;
    }
    public override void Exit()
    {
        GameManager.Instance.player.playerMove.PlusStamina = 10;
        GameManager.Instance.player.playerMove.MinusStamina = 5;
    }
}
public class OintmentItemStrategy : EquipItemStrategy
{
    EquipmentItem equip;
    IEnumerator hpPlusCo;
    public OintmentItemStrategy(EquipmentItem equipmentItem) : base(equipmentItem)
    {
        this.equip = equipmentItem;
        Init();
    }

    public override void Init()
    {
        explanationText = "��� ������ �ǰ� ���� ���ϴ�";
        equip.ExplanationText = this.explanationText;
        hpPlusCo = equipmentItem.HpPlusCo();
    }
    public override void Use()
    {
        equipmentItem.StartCoroutine(hpPlusCo);
    }
    public override void Exit()
    {
        equipmentItem.StopCoroutine(hpPlusCo);
    }
}
public class MaskItemStrategy : EquipItemStrategy
{
    EquipmentItem equip;
    public MaskItemStrategy(EquipmentItem equipmentItem) : base(equipmentItem)
    {
        this.equip = equipmentItem;
        Init();
    }

    public override void Init()
    {
        explanationText = "���͸� ���� �� �ټ��� �پ��� ������ �پ��ϴ�";
        equip.ExplanationText = this.explanationText;
    }
    public override void Use()
    {
        GameManager.Instance.player.MonsterLookZone = 5;
    }
    public override void Exit()
    {
        GameManager.Instance.player.MonsterLookZone = 10;
    }
}
public class NightVisionStrategy : EquipItemStrategy
{
    EquipmentItem equip;
    public NightVisionStrategy(EquipmentItem equipmentItem) : base(equipmentItem)
    {
        this.equip = equipmentItem;
        Init();
    }

    public override void Init()
    {
        explanationText = "�̸� : �߰����ð� �� �� �ִ� �þ߹����� �о����ϴ�.";
        equip.ExplanationText = this.explanationText;
    }
    public override void Use()
    {
        GameManager.Instance.player.MonsterLookZone = 5;
    }
    public override void Exit()
    {
        GameManager.Instance.player.MonsterLookZone = 10;
    }
}
public enum EQUIPITEM_TYPE
{
    OINTMENT,
    ADRENALINE,
    MASK,
    NIGHTVISION,
}
public class EquipmentItem : Item
{
    public EQUIPITEM_TYPE equipItem_Type;
    public Image batteryCharge;
    private void Start()
    {
        Init();
    }
    public override void Init()
    {
        switch (equipItem_Type)
        {
            case EQUIPITEM_TYPE.OINTMENT:
                itemStrategy = new OintmentItemStrategy(this);
                break;
            case EQUIPITEM_TYPE.ADRENALINE:
                itemStrategy = new AdrenalineItemStrategy(this);
                break;
            case EQUIPITEM_TYPE.MASK:
                itemStrategy = new MaskItemStrategy(this);
                break;
            case EQUIPITEM_TYPE.NIGHTVISION:
                itemStrategy = new MaskItemStrategy(this);
                break;
        }
    }

    public override void Exit()
    {
        itemStrategy.Exit();
    }
    public override void Active()
    {
        EquipItemInventory equipInven = GameManager.Instance.player.EquipInven;
        equipInven.AddItem(this);
    }
    public IEnumerator HpPlusCo()
    {
        while (GameManager.Instance.player.Hp <= 100)
        {
            yield return new WaitForSeconds(30);
            GameManager.Instance.player.Hp += 5;
            yield return new WaitUntil(() => GameManager.Instance.player.Hp <= 100f);
        }
    }
}