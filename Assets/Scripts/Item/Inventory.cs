using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Slot[] equipSlot = new Slot[5];
    [SerializeField] private Slot tempSlot;
    [SerializeField] private Slot playerEquipSlot;
    [SerializeField] private Image textCoverImage;

    private int index;
    public Slot[] EquipSlot
    {
        get { return equipSlot; }
        set { equipSlot = value; }
    }
    public Slot PlayerEquipSlot
    {
        get => playerEquipSlot;
    }
    public void IndexSlot(int index)
    {
        this.index = index;
        for(int i = 0; i < equipSlot.Length; i++)
        {
            if(i == index)
            {
                if (equipSlot[i].item == null)
                    return;
                equipSlot[i].GetComponent<Image>().color = Color.yellow;
                textCoverImage.gameObject.SetActive(true);
                textCoverImage.GetComponentInChildren<TextMeshProUGUI>().text= equipSlot[i].item.ExplanationText;
            }
            else
            {
                equipSlot[i].GetComponent<Image>().color = Color.red;
                textCoverImage.gameObject.SetActive(false);
            }
        }
    }
    public void ResetSlot()
    {
        Debug.Log("¸®¼Âµé¾î¿È");
        for (int j = 0; j < equipSlot.Length; j++)
        {
            equipSlot[j].GetComponent<Image>().color = Color.red;
        }
    }
    public void SwitchItem()
    {
        if (playerEquipSlot.item != null)
            playerEquipSlot.item.Exit();
        tempSlot.item = equipSlot[index].item;
        tempSlot.GetComponent<Image>().sprite = equipSlot[index].GetComponent<Image>().sprite;
        equipSlot[index].item = playerEquipSlot.item;
        equipSlot[index].GetComponent<Image>().sprite = playerEquipSlot.GetComponent<Image>().sprite;
        playerEquipSlot.item = tempSlot.item;
        playerEquipSlot.GetComponent<Image>().sprite = tempSlot.GetComponent<Image>().sprite;
        tempSlot.item = null;
        tempSlot.GetComponent<Image>().sprite = null;
        ItemBuff();
    }
    public void ItemBuff()
    {
        playerEquipSlot.item.Use();
    }
}
