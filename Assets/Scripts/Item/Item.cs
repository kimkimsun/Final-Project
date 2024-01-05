using UnityEngine;
using System;
using CustomInterface;

public abstract class ItemStrategy
{
    protected Item item;
    public ItemStrategy(Item item)
    {
        this.item = item;
    }
    public abstract void Use();
}


public class Item : MonoBehaviour, IActivable
{
    protected ItemStrategy itemStrategy = null;
    protected Action action;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(itemStrategy);
            itemStrategy.Use();
        }
    }
    public Action Active()
    {
        return action += () => {itemStrategy.Use();};
    }
}