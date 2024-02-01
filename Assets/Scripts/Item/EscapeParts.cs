using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PARTS_TYPE
{
    WHEEL,
    HANDLE,
    CARKEY,
    OIL,
}
public enum FIXOBJ_TYPE
{
    NONE,
    SPANNER,
    DRIVER
}
public class EscapeParts : MonoBehaviour,IInteraction
{
    public PARTS_TYPE partsType;
    public FIXOBJ_TYPE fixType;
    public Sprite sprite;
    private PartsSlot[] parts;
    public Dictionary<PARTS_TYPE,FIXOBJ_TYPE> fixedDic = new Dictionary<PARTS_TYPE,FIXOBJ_TYPE>();
    public string InteractionText => "ащ╠Б";
    public void Active()
    {
        GameManager.Instance.player.partsInven.AddParts(this);
    }

    private void Start()
    {
        parts = GameManager.Instance.player.partsInven.PartsSlots;
        switch (partsType)
        {
            case PARTS_TYPE.WHEEL:
                partsType = PARTS_TYPE.WHEEL;
                fixedDic.Add(PARTS_TYPE.WHEEL, FIXOBJ_TYPE.SPANNER);
                break;
            case PARTS_TYPE.HANDLE:
                partsType = PARTS_TYPE.HANDLE;
                fixedDic.Add(PARTS_TYPE.HANDLE, FIXOBJ_TYPE.DRIVER);
                break;
            case PARTS_TYPE.CARKEY:
                partsType = PARTS_TYPE.CARKEY;
                fixedDic.Add(PARTS_TYPE.HANDLE, FIXOBJ_TYPE.NONE);
                break;
            case PARTS_TYPE.OIL:
                partsType = PARTS_TYPE.OIL;
                fixedDic.Add(PARTS_TYPE.HANDLE, FIXOBJ_TYPE.NONE);
                break;
        }
        switch (fixType)
        {
            case FIXOBJ_TYPE.NONE:
                break;
            case FIXOBJ_TYPE.SPANNER:
                fixType = FIXOBJ_TYPE.SPANNER;
                break;
            case FIXOBJ_TYPE.DRIVER:
                fixType = FIXOBJ_TYPE.DRIVER;
                break;
        }
    }
    public bool PairCheck(PARTS_TYPE partsType, FIXOBJ_TYPE fixType)
    {
        if (fixedDic[partsType].Equals(fixType)) 
            return true;
     return false;
    }
}