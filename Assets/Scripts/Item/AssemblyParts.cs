using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public enum ASSEMBLE_TYPE
{
    LEFTWHEELPOINT,
    RIGHTWHEELPOINT,
    HANDLEPOINT,
    CARKEYPOINT,
    OILPOINT,
}
public class AssemblyParts : MonoBehaviour, IInteraction
{
    public ASSEMBLE_TYPE assembleType;
    public List<Image> imagePair = new List<Image>();

    private string interactionText;
    private int itemIndex;
    private Dictionary<PARTS_TYPE, ASSEMBLE_TYPE> partsCombinationDic = new Dictionary<PARTS_TYPE, ASSEMBLE_TYPE>();
    private PartsSlot[] parts;
    public string InteractionText => interactionText;

    public void Fit(ASSEMBLE_TYPE type)
    {
        type = this.assembleType;
        for(int i = 0;  i < parts.Length; i++)
        {
            if (partsCombinationDic.ContainsKey(parts[i].parts.partsType))
                Active();
        }
    }
    public void Active()
    {
        parts[itemIndex].parts.gameObject.SetActive(true);
        parts[itemIndex].parts.gameObject.transform.position = transform.position;
        parts[itemIndex].parts.gameObject.transform.SetParent(gameObject.transform);
        parts[itemIndex].parts = null;
        parts[itemIndex].itemImage.sprite = null;
        for(int i =0; i < imagePair.Count;i++)
        {
            imagePair[i].gameObject.SetActive(false);
        }
        Destroy(gameObject);
    }

    private void Start()
    {
        parts = GameManager.Instance.player.partsInven.PartsSlots;
        switch (assembleType)
        {
            case ASSEMBLE_TYPE.LEFTWHEELPOINT:
                assembleType = ASSEMBLE_TYPE.LEFTWHEELPOINT;
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
        }
    }
}
