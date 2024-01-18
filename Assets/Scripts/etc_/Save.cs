using CustomInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
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
    public UseItemSlot[] saveUseItemSlot;
    public SaveData(Player player, HiRil hiril, HaiKen haiken, UseItemSlot[] useItemSlot)
    {
        hp = player.Hp;
        tension = player.Tension;
        stamina = player.Stamina;
        playerPos = player.transform.position;
        playerRot = player.transform.rotation.eulerAngles;
        hirilPos = hiril.transform.position;
        hirilRot = hiril.transform.rotation.eulerAngles;
        haikenPos = haiken.transform.position;
        haikenRot = haiken.transform.root.eulerAngles;
        saveUseItemSlot = useItemSlot;

        for(int i = 0; i < useItemSlot.Length; i++)
        {
            for(int j = 0; j < useItemSlot[i].items.Count; i ++)
            {
                saveUseItemSlot[i].items[j] = useItemSlot[i].items[j];
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
        hiril.transform.position = hirilPos;
        hiril.transform.eulerAngles = hirilRot;
        haiken.transform.position = haikenPos;
        haiken.transform.root.eulerAngles = haikenRot;

        for (int i = 0; i < saveUseItemSlot.Length; i++)
        {
            for (int j = 0; j < saveUseItemSlot[i].items.Count; i++)
            {
                useItemSlot[i].items[j] = saveUseItemSlot[i].items[j];
            }
        }

    }
      
        

}

public class Save : MonoBehaviour, IInteraction
{
    SaveData saveData;
    UseItemSlot[]useItemSlot;
    Player player;
    HiRil hiril;
    HaiKen haiken;
    int index;
    string path;
    string fileName;


    public int Index
    {
         get => Index;
         set
        {
            index = value;
            if (index < 2)
                Index = 0;
        }
    
    }
    public string InteractionText => "Save";


    private void Start()
    {
        index = 0;
        player = GameManager.Instance.player;
        hiril = GameManager.Instance.hiril;
        haiken = GameManager.Instance.haiken;
        useItemSlot = GameManager.Instance.player.quickSlot.slots; 

        path = "Assets/";
        fileName = "SaveData" + index + ".txt";
    }

    public void SaveData()
    {
        StreamWriter sw;
        if(File.Exists(path+fileName) == false )
        {
            sw = File.CreateText(path+fileName);
            Index++;
        }
        else
        {
            sw = new StreamWriter(path+fileName);
        }
        sw.Write(JsonUtility.ToJson(saveData));
        sw.Close();
    }

    public void LoadData()
    {
        if(File.Exists(path+fileName))
        {
            StreamReader sr =new StreamReader(path+fileName);
            saveData = JsonUtility.FromJson<SaveData>(sr.ReadToEnd());
            sr.Close();
            saveData.Load(player, hiril, haiken, useItemSlot);
        }
    }

    private void FixedUpdate()//재영공신
    {
        
        if(Input.GetKeyDown(KeyCode.V))
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
