using System.Collections;
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
public class FlashlightItemStrategy : EquipItemStrategy
{
    EquipmentItem equip;
    Light flashlight;
    IEnumerator minusBatteryCo;
    IEnumerator plusBatteryCo;
    int minBright;
    int maxBright;
    int battery;
    int minBattery;
    int maxBattery;
    bool useing;

    public int Battery
    {
        get { return battery; }
        set
        {
            battery = value;
            if (battery <= minBattery)
                battery = minBattery;
            if (battery >= maxBattery)
                battery = maxBattery;
        }
    }
    public FlashlightItemStrategy(EquipmentItem equipmentItem) : base(equipmentItem)
    {
        this.equip = equipmentItem;
        Init();
    }
    public override void Init()
    {
        minBright = 0;
        maxBright = 10;
        minBattery = 0;
        maxBattery = 60;
        Battery = 60;
        minusBatteryCo = MinusBatteryCo();
        plusBatteryCo = PlusBatteryCo();

        flashlight = equipmentItem.GetComponentInChildren<Light>();
        flashlight.intensity = minBright;
    }
    public override void Use()
    {
        useing = !useing;
        if (useing)
        {
            equipmentItem.StopCoroutine(plusBatteryCo);
            equipmentItem.StartCoroutine(minusBatteryCo);
        }
        else
            Exit();
    }

    public override void Exit()
    {
        equipmentItem.StopCoroutine(minusBatteryCo);
        equipmentItem.StartCoroutine(plusBatteryCo);
    }
    IEnumerator MinusBatteryCo()
    {
        while (Battery > minBattery)
        {
            flashlight.intensity = maxBright;
            yield return new WaitForSeconds(1f);
            equipmentItem.batteryCharge.fillAmount = Battery / 60f;
            Battery -= 1;
            if (Battery <= minBattery)
            {
                Exit();
            }
            yield return new WaitUntil(() => Battery > minBattery);
        }
    }

    IEnumerator PlusBatteryCo()
    {
        while (Battery < maxBattery)
        {
            flashlight.intensity = minBright;
            yield return new WaitForSeconds(0.6f);
            equipmentItem.batteryCharge.fillAmount = Battery / 60f;
            battery += 1;
            Debug.Log("�÷���");
            yield return new WaitUntil(() => Battery < maxBattery);
        }
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
    FLASHLIGHT,
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
            case EQUIPITEM_TYPE.FLASHLIGHT:
                itemStrategy = new FlashlightItemStrategy(this);
                break;
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
        while(GameManager.Instance.player.Hp <= 100)
        {
            yield return new WaitForSeconds(30);
            GameManager.Instance.player.Hp += 5;
            yield return new WaitUntil(() => GameManager.Instance.player.Hp <= 100f);
        }
    }
}