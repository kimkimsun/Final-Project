using CustomInterface;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, IStorable
{

    [SerializeField] private InvenSlot tempSlot;
    //[SerializeField] private Slot[] portableSlot = new Slot[2];
    [SerializeField] private InvenSlot[] equipQuickSlot = new InvenSlot[4];
    [SerializeField] private InvenSlot playerEquipSlot;
    [SerializeField] private Image textCoverImage;

    private int index;

    public int Index { get => index;}

   /* public Slot[] equipQuickSlot
    {
        get { return equipQuickSlot; }
        set { equipQuickSlot = value; }
    }
    public Slot playerEquipSlot
    {
        get => playerEquipSlot;
    }*/
    public void AddItem(Item item)
    {


        if (playerEquipSlot.item == null)
        {
            playerEquipSlot.item = item;
            item.Use();
            item.gameObject.SetActive(false);
        }
        else
        {
            for (int i = equipQuickSlot.Length - 1; i >= 1; i--)
            {
                if (equipQuickSlot[i].item == null)
                {
                    equipQuickSlot[i].item = item;
                    equipQuickSlot[i].GetComponent<Image>().sprite = item.sprite;
                    item.gameObject.SetActive(false);
                    break;
                }
            }
        }


    }
    public void IndexSlot(int index)
    {
        this.index = index;
        for(int i = 0; i < equipQuickSlot.Length; i++)
        {
            if(i == index)
            {
                equipQuickSlot[i].GetComponent<Image>().color = Color.yellow;
                if (equipQuickSlot[i].item != null && GameManager.Instance.player.Inven.gameObject.activeSelf)
                {
                    textCoverImage.gameObject.SetActive(true);
                    textCoverImage.GetComponentInChildren<TextMeshProUGUI>().text = equipQuickSlot[i].item.ExplanationText;
                }
            }
            else
            {
                Debug.Log("TSST" + i);
                equipQuickSlot[i].GetComponent<Image>().color = Color.red;
                textCoverImage.gameObject.SetActive(false);
            }
        }
    }
    public void SwitchItem()
    {
        if (playerEquipSlot.item != null)
            playerEquipSlot.item.Exit();
        if (equipQuickSlot[index].item == null)
            return;
        tempSlot.item = equipQuickSlot[index].item;
        tempSlot.GetComponent<Image>().sprite = equipQuickSlot[index].GetComponent<Image>().sprite;

        equipQuickSlot[index].item = playerEquipSlot.item;
        equipQuickSlot[index].GetComponent<Image>().sprite = playerEquipSlot.GetComponent<Image>().sprite;

        playerEquipSlot.item = tempSlot.item;
        playerEquipSlot.GetComponent<Image>().sprite = tempSlot.GetComponent<Image>().sprite;

        tempSlot.item = null;
        tempSlot.GetComponent<Image>().sprite = null;
       // playerEquipSlot.item.Use();
    }

}