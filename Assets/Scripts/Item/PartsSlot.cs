using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartsSlot : Slot
{
    public EscapeParts parts;
    public void SetParts(EscapeParts parts)
    {
        this.parts = parts;
        //parts.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        this.itemImage.sprite = parts.sprite;
        parts.transform.SetParent(GameManager.Instance.player.escapeBox.transform);
        parts.gameObject.SetActive(false);
    }
}