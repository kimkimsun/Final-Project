using UnityEngine;
using CustomInterface;
using UnityEngine.UI;
using System;
public abstract class ItemStrategy
{
    protected Item item;
    public string explanationText;
    public ItemStrategy itemstrategy;
    public abstract void Use();
    public virtual void Init() { }
    public virtual void Exit() { }
}
[Serializable]
public abstract class Item : MonoBehaviour, IInteraction
{
    public ItemStrategy itemStrategy = null;
    public string itemName;
    public Sprite sprite;
    public int itemID;
    private string interactionText;
    public string explanationText;

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
        interactionText = "�ݱ�";
    }
    public virtual void Use()
    {
        itemStrategy.Use();
    }
    public virtual void Exit() { }
    public virtual void Active() { }
    public abstract void Init();
}