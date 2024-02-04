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
    private bool isParts;
    private static bool isTireLeft;
    private static bool isTireRight;
    private static bool isHandle;
    private static bool isOil;
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
        if(assembleType is not ASSEMBLE_TYPE.CARKEYPOINT || assembleType is not ASSEMBLE_TYPE.OILPOINT)
        {
            parts[itemIndex].parts.gameObject.SetActive(true);
            parts[itemIndex].parts.gameObject.transform.position = transform.position;
            parts[itemIndex].parts.gameObject.transform.rotation = transform.rotation;
            parts[itemIndex].parts.gameObject.transform.SetParent(null);
            parts[itemIndex].parts = null;
            parts[itemIndex].itemImage.sprite = null;
            isParts = true;
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
                assembleType = ASSEMBLE_TYPE.LEFTWHEELPOINT;
                partsCombinationDic.Add(PARTS_TYPE.LEFTWHEEL, ASSEMBLE_TYPE.LEFTWHEELPOINT);
                interactionText = "���� ����";
                BoolSetting(ref isTireLeft);
                break;
            case ASSEMBLE_TYPE.RIGHTWHEELPOINT:
                assembleType = ASSEMBLE_TYPE.RIGHTWHEELPOINT;
                partsCombinationDic.Add(PARTS_TYPE.RIGHTWHEEL, ASSEMBLE_TYPE.RIGHTWHEELPOINT);
                interactionText = "������ ����";
                BoolSetting(ref isTireRight);
                break;
            case ASSEMBLE_TYPE.HANDLEPOINT:
                assembleType = ASSEMBLE_TYPE.HANDLEPOINT;
                partsCombinationDic.Add(PARTS_TYPE.HANDLE, ASSEMBLE_TYPE.HANDLEPOINT);
                interactionText = "�ڵ�";
                BoolSetting(ref isHandle);
                break;
            case ASSEMBLE_TYPE.CARKEYPOINT:
                assembleType = ASSEMBLE_TYPE.CARKEYPOINT;
                partsCombinationDic.Add(PARTS_TYPE.CARKEY, ASSEMBLE_TYPE.CARKEYPOINT);
                interactionText = "��Ű";
                //BoolSetting(ref isTireLeft);
                break;
            case ASSEMBLE_TYPE.OILPOINT:
                assembleType = ASSEMBLE_TYPE.OILPOINT;
                partsCombinationDic.Add(PARTS_TYPE.OIL, ASSEMBLE_TYPE.OILPOINT);
                interactionText = "�⸧��";
                BoolSetting(ref isOil);
                break;
        }

    }
    private void BoolSetting(ref bool A)
    {
        A = isParts = A;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log(isParts);
            Debug.Log(isTireLeft);
        }
    }
}
