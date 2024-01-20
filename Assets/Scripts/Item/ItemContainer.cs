using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : MonoBehaviour, IInteraction
{
    public string InteractionText => "open";
    [SerializeField] private Animator ContainerAni;
    private bool isOpen;

    private void Start()
    {
        isOpen = false;
    }
    public bool IsOpen
    {

        get => isOpen;
        set
        {
            isOpen = value;
            if (isOpen)
            {
                ContainerAni.SetBool("IsOpen", true);
            }
            else
            {
                ContainerAni.SetBool("IsOpen", false);
            }
        }


    }


    public void Active()
    {
        IsOpen = !IsOpen;
    }

}
