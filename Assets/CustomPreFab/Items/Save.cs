using CustomInterface;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
public class SaveData
{
    public int hp;
    public int tension;
    public float stamina;
    public Vector3 playerPos;
    public Vector3 playerRot;
    public Vector3 hirilPos;
    public Vector3 hirilRot;
    public Vector3 haikenPos;
    public Vector3 haikenRot;
    public UseItemSlot[] save;
    public int[] equipIndexArray = new int[Save.Instance.player.equipInven.EiSlots.Length];
    public int[] useItemCount = new int[Save.Instance.player.quickSlot.slots.Length];
    public int[] useItemIndexArray = new int[Save.Instance.player.quickSlot.slots.Length];
    public int equipInvenIdIndex;
    public int hairPinCount;

    public SaveData(Player player, HiRil hiril, HaiKen haiken, UseItemSlot[] useItemSlot)
    {
        hp = player.Hp;
        tension = player.Tension;
        stamina = player.Stamina;
        playerPos = player.transform.position;
        playerRot = player.transform.rotation.eulerAngles;
        save = useItemSlot;
        if (player.equipInven.EquipSlot != null)
            equipInvenIdIndex = player.equipInven.EquipSlot.item.itemID;
        if (player.quickSlot.hairPinSlot.items.Count > 0)
            hairPinCount = player.quickSlot.hairPinSlot.items.Count;
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
                Debug.Log("카운트 " + player.quickSlot.slots[j].items.Count);
                useItemIndexArray[j] = player.quickSlot.slots[j].items[0].itemID;
            }
        }

    }
    public void Load(Player player, HiRil hiril, HaiKen haiken, UseItemSlot[] useItemSlot)
    {
        player.Hp = hp;
        player.Tension = tension;
        player.Stamina = stamina;
        player.transform.position = playerPos;
        player.transform.eulerAngles = playerRot;
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
                            player.quickSlot.slots[i].items.Add(ItemManager.Instance.itemList[j]);
                        }
                    }
                }
            }
        }
        for (int i = 0; i < ItemManager.Instance.itemList.Count; i++)
        {
            if (equipInvenIdIndex == ItemManager.Instance.itemList[i].itemID)
                player.equipInven.EquipSlot.item = ItemManager.Instance.itemList[i];
        }
        if (hairPinCount > 0)
        {
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
    public static Save Instance;

    SaveData saveData;
    UseItemSlot[] useItemSlot;
    public Player player;
    HiRil hiril;
    HaiKen haiken;
    int index;
    string path;
    string fileName;


    //public int Index
    //{
    //     get => Index;
    //     set
    //    {
    //        index = value;
    //        if (index < 2)
    //            Index = 0;
    //    }

    //}
    public string InteractionText => "Save";

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        index = 0;
        player = GameManager.Instance.player;
        hiril = GameManager.Instance.hiril;
        haiken = GameManager.Instance.haiken;
        useItemSlot = GameManager.Instance.player.quickSlot.slots;

        path = "Assets/";
        fileName = "SaveData.txt";
    }

    public void SaveData()
    {
        Debug.Log("세이브");
        StreamWriter sw;
        if (File.Exists(path + fileName) == false)
        {
            sw = File.CreateText(path + fileName);
            //Index++;
        }
        else
        {
            sw = new StreamWriter(path + fileName);
        }
        sw.Write(JsonUtility.ToJson(saveData, true));
        sw.Close();
    }

    public void LoadData()
    {
        Debug.Log("로드");
        if (File.Exists(path + fileName))
        {
            StreamReader sr = new StreamReader(path + fileName);
            saveData = JsonUtility.FromJson<SaveData>(sr.ReadToEnd());
            sr.Close();
            saveData.Load(player, hiril, haiken, useItemSlot);
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            LoadData();
        }
    }

    public void Active()
    {
        saveData = new SaveData(player, hiril, haiken, useItemSlot);
        SaveData();
    }
}