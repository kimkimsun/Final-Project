using UnityEngine;
using System;
using CustomInterface;

public abstract class ItemStrategy
{
    protected Item item;

    public abstract void Use();
    public virtual void Exit() { }
}


public abstract class Item : MonoBehaviour, IInteraction
{
    protected ItemStrategy itemStrategy = null;    
    private string interactionText;
    private void Awake()
    {
        interactionText = "Get";
    }
    public string InteractionText
    {
        get => interactionText;
    }

    public virtual void Use()
    {
        itemStrategy.Use();
    }
    public abstract void Active(); //ащ╠Б
}