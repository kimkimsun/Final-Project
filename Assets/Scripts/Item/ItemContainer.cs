using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : MonoBehaviour, IInteraction
{
    public string InteractionText => "open";
    [SerializeField] private Animator ContainerAni;
    private bool isOpen;
    private BoxCollider boxCollider;

    private void Start()
    {
        isOpen = false;
        boxCollider = GetComponent<BoxCollider>();
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
                boxCollider.enabled = false;
            }
            else
            {
                ContainerAni.SetBool("IsOpen", false);
                boxCollider.enabled = true;
            }
        }


    }


    public void Active()
    {
        
        IsOpen = !IsOpen;
    }

}
