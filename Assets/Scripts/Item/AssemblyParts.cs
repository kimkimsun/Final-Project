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
    WHEELPOINT,
    HANDLEPOINT,
    CARKEYPOINT,
    OILPOINT,
}
public class AssemblyParts : MonoBehaviour, IInteraction
{
    public ASSEMBLE_TYPE assembleType;
    public Image imagePair;

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
        parts[itemIndex].gameObject.SetActive(true);
        parts[itemIndex].gameObject.transform.position = transform.position;
        parts[itemIndex].parts = null;
        parts[itemIndex].itemImage.sprite = null;
        imagePair.gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void Start()
    {
        parts = GameManager.Instance.player.partsInven.PartsSlots;
        switch (assembleType)
        {
            case ASSEMBLE_TYPE.WHEELPOINT:
                assembleType = ASSEMBLE_TYPE.WHEELPOINT;
                partsCombinationDic.Add(PARTS_TYPE.WHEEL, ASSEMBLE_TYPE.WHEELPOINT);
                interactionText = "바퀴";
                break;
            case ASSEMBLE_TYPE.HANDLEPOINT:
                assembleType = ASSEMBLE_TYPE.HANDLEPOINT;
                partsCombinationDic.Add(PARTS_TYPE.HANDLE, ASSEMBLE_TYPE.HANDLEPOINT);
                interactionText = "핸들";
                break;
            case ASSEMBLE_TYPE.CARKEYPOINT:
                assembleType = ASSEMBLE_TYPE.CARKEYPOINT;
                partsCombinationDic.Add(PARTS_TYPE.CARKEY, ASSEMBLE_TYPE.CARKEYPOINT);
                interactionText = "차키";
                break;
            case ASSEMBLE_TYPE.OILPOINT:
                assembleType = ASSEMBLE_TYPE.OILPOINT;
                partsCombinationDic.Add(PARTS_TYPE.OIL, ASSEMBLE_TYPE.OILPOINT);
                interactionText = "기름통";
                break;
        }
    }
}
