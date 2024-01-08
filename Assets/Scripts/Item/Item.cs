using UnityEngine;
using System;
using CustomInterface;

public abstract class ItemStrategy
{
    protected Item item;
    public string explanationText;
    public abstract void Use();
    public virtual void Init() { }
    public virtual void Exit() { }
}


public abstract class Item : MonoBehaviour, IInteraction
{
    protected ItemStrategy itemStrategy = null;    
    private string interactionText;
    private string explanationText;

    public string ExplanationText
    {
        get => explanationText;
        set => explanationText = value;
    }
    public string InteractionText
    {
        get => interactionText;
    }
    private void Awake()
    {
        interactionText = "Get";
    }

    public virtual void Use()
    {
        itemStrategy.Use();
    }
    public virtual void Exit() { }

    public abstract void Active(); //ащ╠Б
}