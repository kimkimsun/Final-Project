using CustomInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TMPro;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int hp;
    public int tension;
    public float stamina;
    public float time;
    public float battery;
    public Vector3 playerPos;
    public Vector3 playerRot;
    public Vector3 hirilPos;
    public Vector3 hirilRot;
    public Vector3 haikenPos;
    public Vector3 haikenRot;
    public UseItemSlot[] save;
    public int[] equipIndexArray;
    public int[] useItemCount;
    public int[] useItemIndexArray;
    public int equipInvenIdIndex;
    public int hairPinCount;
    public string typeName;


    public SaveData(Player player, HiRil hiril, HaiKen haiken)
    {
        equipIndexArray = new int[player.equipInven.EiSlots.Length];
        useItemCount = new int[player.quickSlot.slots.Length];
        useItemIndexArray = new int[player.quickSlot.slots.Length];

        battery = player.battery;
        hp = player.Hp;
        tension = player.Tension;
        stamina = player.Stamina;
        playerPos = player.transform.position;
        playerRot = player.transform.rotation.eulerAngles;
        time = GameManager.Instance.time;
        if (player.equipInven.EquipSlot.item != null)
            equipInvenIdIndex = player.equipInven.EquipSlot.item.itemID;


        //if (player.quickSlot.hairPinSlot.items.Count > 0 && player.quickSlot.hairPinSlot != null)
        //    hairPinCount = player.quickSlot.hairPinSlot.items.Count;


        for (int i = 0; i < player.equipInven.EiSlots.Length; i++)
        {
            equipIndexArray[i] = -1;
            if (player.equipInven.EiSlots[i].item != null)
                equipIndexArray[i] = player.equipInven.EiSlots[i].item.itemID;
        }


        for (int j = 0; j < player.quickSlot.slots.Length; j++)
        {
            useItemIndexArray[j] = -1;
            useItemCount[j] = -1;
            if (player.quickSlot.slots[j].items.Count > 0)
            {
                useItemCount[j] = player.quickSlot.slots[j].items.Count;
                useItemIndexArray[j] = player.quickSlot.slots[j].items[0].itemID;
            }
        }

    }
    public void Load(Player player, HiRil hiril, HaiKen haiken)
    {
        player.Hp = hp;
        player.Tension = tension;
        player.Stamina = stamina;
        player.battery = battery;
        player.transform.position = playerPos;
        player.transform.eulerAngles = playerRot;
        GameManager.Instance.time = time;
        for (int i = 0; i < player.equipInven.EiSlots.Length; i++)
        {
            if (equipIndexArray[i] != -1)
            {
                for (int j = 0; j < ItemManager.Instance.itemList.Count; j++)
                {
                    if (equipIndexArray[i] == ItemManager.Instance.itemList[j].itemID)
                        player.equipInven.EiSlots[i].item = ItemManager.Instance.itemList[j];
                }
            }
        }

        for(int i = 0; i < player.itemBox.transform.childCount; i++)
        {
            GameObject.Destroy(player.itemBox.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < player.quickSlot.slots.Length; i++)
        {
            player.quickSlot.slots[i].items.Clear();
            if (useItemIndexArray[i] != -1)
            {
                for (int j = 0; j < ItemManager.Instance.itemList.Count; j++)
                {
                    if (useItemIndexArray[i] == ItemManager.Instance.itemList[j].itemID)
                    {
                        for (int k = 0; k < useItemCount[i]; k++)
                        {
                            Item copyItem = ItemManager.Instance.CreatePrefab(useItemIndexArray[i]);
                            player.quickSlot.slots[i].items.Add(copyItem);
                            player.quickSlot.slots[i].items[k].Init();
                            player.quickSlot.slots[i].items[k].gameObject.transform.SetParent(player.itemBox.transform);
                            player.quickSlot.slots[i].items[k].gameObject.transform.position = player.itemBox.transform.position;
                            player.quickSlot.slots[i].items[k].gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
        for (int i = 0; i < ItemManager.Instance.itemList.Count; i++)
        {
            if (equipInvenIdIndex == ItemManager.Instance.itemList[i].itemID)
            {
                player.equipInven.EquipSlot.item = ItemManager.Instance.itemList[i];
                player.equipInven.EquipSlot.item.Init();
                player.equipInven.EquipSlot.item.Use();
            }
        }

        if (hairPinCount > 0)
        {
            player.quickSlot.hairPinSlot.items.Clear();
            for (int i = 0; i < hairPinCount; i++)
                player.quickSlot.hairPinSlot.items.Add(ItemManager.Instance.itemList[4/*헤어핀이 배열 몇번째에 위치하는지를 적는게 연산적으로 괜춘 json은 기획이라고 하셨으니 이렇게 해도 됨*/]);
        }
        //useItemSlot = JsonUtility.FromJson<UseItemSlot>(save);
        //hiril.transform.position = hirilPos;
        //hiril.transform.eulerAngles = hirilRot;
        //haiken.transform.position = haikenPos;
        //haiken.transform.root.eulerAngles = haikenRot;
    }
}

public class Save : MonoBehaviour, IInteraction
{
    SaveData saveData;
    UseItemSlot[] useItemSlot;
    public Player player;
    HiRil hiril;
    HaiKen haiken;
    public int fileIndex;
    string path;
    string fileName;
    public static Save Instance;
    public TextMeshProUGUI settingUI;
    public string InteractionText => "Save";
    public void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        player = GameManager.Instance.player;
        hiril = GameManager.Instance.hiril;
        haiken = GameManager.Instance.haiken;
        useItemSlot = GameManager.Instance.player.quickSlot.slots;

        path = "Assets/";
        fileName = "SaveData.txt";
    }

    public void SaveData(int fileIndex)
    {
        settingUI.text = GameManager.Instance.time.ToString("N2");
        fileName = fileName.Insert(8,fileIndex.ToString());
        StreamWriter sw;
        if (File.Exists(path + fileName) == false)
        {
            sw = File.CreateText(path + fileName);
        }
        else
        {
            sw = new StreamWriter(path + fileName);
        }
        sw.Write(JsonUtility.ToJson(saveData, true));
        sw.Close();
    }

    public void LoadData(int fileIndex)
    {
        fileName = fileName.Insert(8, fileIndex.ToString());
        Debug.Log("로드");
        if (File.Exists(path + fileName))
        {
            StreamReader sr = new StreamReader(path + fileName);
            saveData = JsonUtility.FromJson<SaveData>(sr.ReadToEnd());
            sr.Close();
            saveData.Load(player, hiril, haiken);
        }
    }
    public void Active()
    {
        UIManager.Instance.saveUI.gameObject.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void SaveButton()
    {
        saveData = new SaveData(player, hiril, haiken);
        SaveData(fileIndex);
    }
    public void LoadButton()
    {
        LoadData(fileIndex);
    }
}