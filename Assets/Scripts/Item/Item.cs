using UnityEngine;
using System;
using CustomInterface;

public abstract class ItemStrategy
{
    protected Item item;
    protected UseItem useItem;
    protected EquipmentItem equipmentItem;
    public abstract void Use();
}


public class Item : MonoBehaviour, IActivable
{
    protected ItemStrategy itemStrategy = null;    
    public string interactionText;
    public string InteractionText
    {
        get => interactionText;
    }

    public void Active()
    {
        itemStrategy.Use();
    }

}