using UnityEngine;
using System;
using CustomInterface;

public abstract class ItemStrategy
{
    protected Item item;
    protected UseItem useItem;
    protected EquipmentItem equipmentItem;
    public abstract void Use();
    public virtual void Exit()
    {

    }
}


public class Item : MonoBehaviour, IInteraction
{
    protected ItemStrategy itemStrategy = null;    
    public string interactionText;
    private void Start()
    {
        interactionText = "ащ╠Б";
    }
    public string InteractionText
    {
        get => interactionText;
    }

    public void Active()
    {
        itemStrategy.Use();
    }
}