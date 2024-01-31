using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
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
        explanationText = "스테미너가 더 느리게 달고 더 빠르게 찹니다.";
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
        explanationText = "들고 있으면 피가 점점 찹니다";
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
        explanationText = "몬스터를 봤을 때 텐션이 줄어드는 범위가 줄어듭니다";
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
public class NightVisionItemStrategy : EquipItemStrategy
{
    EquipmentItem equip;
    //PostProcessProfile profile;
    public NightVisionItemStrategy(EquipmentItem equipmentItem) : base(equipmentItem)
    {
        this.equip = equipmentItem;
        Init();
    }

    public override void Init()
    {
        explanationText = "이름 : 야간투시경 볼 수 있는 시야범위가 넓어집니다.";
        equip.ExplanationText = this.explanationText;
        //profile = Camera.main.GetComponent<PostProcessVolume>().profile;
    }
    public override void Use()
    {
        //profile.GetComponent<Vignette>().intensity = new FloatParameter { value = 0.4f };
    }
    public override void Exit()
    {
        //profile.GetComponent<Vignette>().intensity = new FloatParameter { value = 0.7f };
    }
}
public class FinalKeyItemStrategy : EquipItemStrategy
{
    EquipmentItem equip;
    public FinalKeyItemStrategy(EquipmentItem equipmentItem) : base(equipmentItem)
    {
        this.equip = equipmentItem;
        Init();
    }

    public override void Init()
    {
    }
    public override void Use()
    {
    }
    public override void Exit()
    {
    }
}

public enum EQUIPITEM_TYPE
{
    OINTMENT,
    ADRENALINE,
    MASK,
    NIGHTVISION,
    FINALKEY,
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
                itemStrategy = new NightVisionItemStrategy(this);
                break;
            case EQUIPITEM_TYPE.FINALKEY:
                itemStrategy = new FinalKeyItemStrategy(this);
                break;
        }
    }

    public override void Exit()
    {
        itemStrategy.Exit();
    }
    public override void Active()
    {
        EscapeItemInventory equipInven = GameManager.Instance.player.EquipInven;
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
