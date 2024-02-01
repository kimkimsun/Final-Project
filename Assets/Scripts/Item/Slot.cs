using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Image itemImage;
    public virtual void SetItem(Item setItem) { }
    public virtual void SlotItemUse() { }
}