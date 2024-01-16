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
            Debug.Log("플러스");
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
        explanationText = "스테미너가 더 느리게 달고 더 빠르게 찹니다.";
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
    ADRENALINE,
    FLASHLIGHT,
}
public class EquipmentItem : Item
{
    public EQUIPITEM_TYPE equipItem_Type;
    public Image batteryCharge;
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
            case EQUIPITEM_TYPE.FLASHLIGHT:
                itemStrategy = new FlashlightItemStrategy(this);
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
}