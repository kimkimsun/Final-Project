using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] Slot[] slots = new Slot[5];
    [SerializeField] private Slot[] equipQuickSlot = new Slot[4];
    [SerializeField] private Slot tempSlot;
    [SerializeField] private Slot playerEquipSlot;
    [SerializeField] private Image textCoverImage;

    private int index;

    public int Index
    { get => index;}

    public Slot[] EquipQuickSlot
    {
        get { return equipQuickSlot; }
        set { equipQuickSlot = value; }
    }
    public Slot PlayerEquipSlot
    {
        get => playerEquipSlot;
    }
    public void AddItem(Item item)
    {
        if (item.TryGetComponent<EquipmentItem>(out EquipmentItem eQ))
        {
            if (PlayerEquipSlot.item == null)
            {
                PlayerEquipSlot.item = item;
                PlayerEquipSlot.GetComponent<Image>().sprite = item.sprite;
               // item.Use();
                item.gameObject.SetActive(false);
            }
            else
            {
                for (int i = EquipQuickSlot.Length - 1; i >= 1; i--)
                {
                    if (EquipQuickSlot[i].item == null)
                    {
                        EquipQuickSlot[i].item = item;
                        EquipQuickSlot[i].GetComponent<Image>().sprite = item.sprite;
                        item.gameObject.SetActive(false);
                        break;
                    }
                }
            }
        }
        else if (item.TryGetComponent<UseItem>(out UseItem uI))
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].items.Count != 0 && slots[i].items[slots[i].CurItem].itemName == item.itemName)
                {
                    Debug.Log("들어왔던 아이템");
                    slots[i].items.Add(item);
                    slots[i].CountItem++;
                    slots[i].CurItem++;
                    return;
                }
                else if (slots[i].items.Count == 0)
                {
                    Debug.Log("처음들어온 아이템");
                    slots[i].items.Add(item);
                    slots[i].SetImage(item);
                    slots[i].CountItem++;
                    return;
                }
            }

        }
    }
    public void QuickItemUse()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            slots[0].SlotItemUse();
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            slots[1].SlotItemUse();
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            slots[2].SlotItemUse();
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            slots[3].SlotItemUse();
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            slots[4].SlotItemUse();
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
    private void Update()
    {
        QuickItemUse();
    }
}