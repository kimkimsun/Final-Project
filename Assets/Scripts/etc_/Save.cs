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
    public SaveData(Player player, HiRil hiril, HaiKen haiken)
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
    }
    public void Load(Player player, HiRil hiril, HaiKen haiken)
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
    }
      
        

}

public class Save : MonoBehaviour, IInteraction
{
    SaveData saveData;
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

        path = "Assets/";
        fileName = "SaveData.txt";
    }

    public void SaveData()
    {
        Debug.Log("세이브");
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
        Debug.Log("로드");
        if(File.Exists(path+fileName))
        {
            StreamReader sr =new StreamReader(path+fileName);
            saveData = JsonUtility.FromJson<SaveData>(sr.ReadToEnd());
            sr.Close();
            saveData.Load(player, hiril, haiken);
        }
    }

    private void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            LoadData();
        }
    }

    public void Active()
    {
        saveData = new SaveData(player, hiril, haiken);
        SaveData();
    }
}
