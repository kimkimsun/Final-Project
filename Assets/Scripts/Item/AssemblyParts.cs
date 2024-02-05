using CustomInterface;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class AssemblyPartsStrategy
{
    public AssemblyParts owner;
    public int itemIndex;
    public AssemblyPartsStrategy(AssemblyParts owner) { this.owner = owner; }
    public virtual void Fit() 
    {
        for (int i = 0; i < owner.parts.Length; i++)
        {
            if (owner.partsCombinationDic.ContainsKey(owner.parts[i].parts.partsType))
            {
                itemIndex = i;
                Active();
            }
        }
    }
    public virtual void Active()
    {
        owner.parts[itemIndex].parts.gameObject.SetActive(true);
        owner.parts[itemIndex].parts.gameObject.transform.position = owner.transform.position;
        owner.parts[itemIndex].parts.gameObject.transform.rotation = owner.transform.rotation;
        owner.parts[itemIndex].parts.gameObject.transform.SetParent(null);
        owner.parts[itemIndex].parts = null;
        owner.parts[itemIndex].itemImage.sprite = null;
        for (int i = 0; i < owner.imagePair.Count; i++)
        {
            owner.imagePair[i].gameObject.SetActive(false);
        }
        GameObject.Destroy(owner.gameObject);
    }
}

public class LeftWheelStrategy : AssemblyPartsStrategy, IInteraction
{
    private string interactionText;
    public LeftWheelStrategy(AssemblyParts owner) : base(owner) 
    {
        owner.partsCombinationDic.Add(PARTS_TYPE.LEFTWHEEL, ASSEMBLE_TYPE.LEFTWHEELPOINT);
        interactionText = "¿ÞÂÊ ¹ÙÄû ²È±â"; 
    }

    public string InteractionText => interactionText;

    public override void Active()
    {
        base.Active();
        AssemblyParts.isTireLeft = true;
    }
}
public class RightWheelStrategy : AssemblyPartsStrategy, IInteraction
{
    private string interactionText;
    public RightWheelStrategy(AssemblyParts owner) : base(owner) 
    { 
        owner.partsCombinationDic.Add(PARTS_TYPE.RIGHTWHEEL, ASSEMBLE_TYPE.RIGHTWHEELPOINT);
        interactionText = "¿À¸¥ÂÊ ¹ÙÄû ²È±â"; 
    }

    public string InteractionText => interactionText;

    public void Active()
    {
    }
}
public class HandleWheelStrategy : AssemblyPartsStrategy, IInteraction
{
    private string interactionText;
    public HandleWheelStrategy(AssemblyParts owner) : base(owner) 
    {
        owner.partsCombinationDic.Add(PARTS_TYPE.HANDLE, ASSEMBLE_TYPE.HANDLEPOINT);
        interactionText = "ÇÚµé ²È±â"; 
    }

    public string InteractionText => interactionText;

    public void Active()
    {
    }
}
public class CarKeyStrategy : AssemblyPartsStrategy, IInteraction
{
    private string interactionText;
    public CarKeyStrategy(AssemblyParts owner) : base(owner) 
    {
        owner.partsCombinationDic.Add(PARTS_TYPE.CARKEY, ASSEMBLE_TYPE.CARKEYPOINT);
        interactionText = "Â÷ Å° ²È±â"; 
    }

    public string InteractionText => interactionText;

    public void Active()
    {
    }
}
public class OilStrategy : AssemblyPartsStrategy, IInteraction
{
    private string interactionText;
    public OilStrategy(AssemblyParts owner) : base(owner) 
    {
        owner.partsCombinationDic.Add(PARTS_TYPE.OIL, ASSEMBLE_TYPE.OILPOINT);
        interactionText = "±â¸§ ³Ö±â"; 
    }

    public string InteractionText => interactionText;

    public void Active()
    {
    }
}
public class GeneratorStrategy : AssemblyPartsStrategy, IInteraction
{
    private string interactionText;
    public GeneratorStrategy(AssemblyParts owner) : base(owner) { interactionText = "¹ßÀü±â µ¹¸®±â"; }

    public string InteractionText => interactionText;

    public void Active()
    {
    }
}
public enum ASSEMBLE_TYPE
{
    LEFTWHEELPOINT,
    RIGHTWHEELPOINT,
    HANDLEPOINT,
    CARKEYPOINT,
    OILPOINT,
    GENERATOR,
}
public class AssemblyParts : MonoBehaviour, IInteraction
{
    public ASSEMBLE_TYPE assembleType;
    public List<Image> imagePair = new List<Image>();
    public Dictionary<PARTS_TYPE, ASSEMBLE_TYPE> partsCombinationDic = new Dictionary<PARTS_TYPE, ASSEMBLE_TYPE>();
    public PartsSlot[] parts;

    private AssemblyPartsStrategy partsStrategy;
    private string interactionText;
    private int itemIndex;
    public static bool isTireLeft;
    public static bool isTireRight;
    public static bool isHandle;
    public static bool isOil;
    public string InteractionText => interactionText;
    public void Fit()
    {
        for(int i = 0;  i < parts.Length; i++)
        {
            if (partsCombinationDic.ContainsKey(parts[i].parts.partsType))
                Active();
        }
    }
    public void Active()
    {
        if(assembleType is not ASSEMBLE_TYPE.CARKEYPOINT || assembleType is not ASSEMBLE_TYPE.OILPOINT)
        {
            parts[itemIndex].parts.gameObject.SetActive(true);
            parts[itemIndex].parts.gameObject.transform.position = transform.position;
            parts[itemIndex].parts.gameObject.transform.rotation = transform.rotation;
            parts[itemIndex].parts.gameObject.transform.SetParent(null);
            parts[itemIndex].parts = null;
            parts[itemIndex].itemImage.sprite = null;
            for (int i = 0; i < imagePair.Count; i++)
            {
                imagePair[i].gameObject.SetActive(false);
            }
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        parts = GameManager.Instance.player.partsInven.PartsSlots;
        switch (assembleType)
        {
            case ASSEMBLE_TYPE.LEFTWHEELPOINT:
                partsStrategy = new LeftWheelStrategy(this);
                partsCombinationDic.Add(PARTS_TYPE.LEFTWHEEL, ASSEMBLE_TYPE.LEFTWHEELPOINT);
                interactionText = "¿ÞÂÊ ¹ÙÄû";
                break;
            case ASSEMBLE_TYPE.RIGHTWHEELPOINT:
                assembleType = ASSEMBLE_TYPE.RIGHTWHEELPOINT;
                partsCombinationDic.Add(PARTS_TYPE.RIGHTWHEEL, ASSEMBLE_TYPE.RIGHTWHEELPOINT);
                interactionText = "¿À¸¥ÂÊ ¹ÙÄû";
                break;
            case ASSEMBLE_TYPE.HANDLEPOINT:
                assembleType = ASSEMBLE_TYPE.HANDLEPOINT;
                partsCombinationDic.Add(PARTS_TYPE.HANDLE, ASSEMBLE_TYPE.HANDLEPOINT);
                interactionText = "ÇÚµé";
                break;
            case ASSEMBLE_TYPE.CARKEYPOINT:
                assembleType = ASSEMBLE_TYPE.CARKEYPOINT;
                partsCombinationDic.Add(PARTS_TYPE.CARKEY, ASSEMBLE_TYPE.CARKEYPOINT);
                interactionText = "Â÷Å°";
                break;
            case ASSEMBLE_TYPE.OILPOINT:
                assembleType = ASSEMBLE_TYPE.OILPOINT;
                partsCombinationDic.Add(PARTS_TYPE.OIL, ASSEMBLE_TYPE.OILPOINT);
                interactionText = "±â¸§Åë";
                break;
            case ASSEMBLE_TYPE.GENERATOR:
                assembleType = ASSEMBLE_TYPE.GENERATOR;
                break;
        }
    }
}