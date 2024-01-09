using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using static UnityEditor.Progress;

public class UseItemInfo : MonoBehaviour
{

    public Image itemImage;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemInfo;


    public void SetInfo(Item item)
    {
        itemImage.sprite = item.sprite;
        itemName.text = item.itemName;
        itemInfo.text = item.explanationText;
    }
}
