using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Slot : MonoBehaviour
{
    public Image itemImage;
    public abstract void SetItem(Item setItem);
    public virtual void SlotItemUse() { }
}