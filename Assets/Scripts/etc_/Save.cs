using CustomInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

[System.Serializable]
public class SaveData
{


    public int hp;
    public int tension;
    public float stamina;
    public string playerPos;
    public string hirilPos;
    public string haikenPos;
    public SaveData(Player player, HiRil hiril , HaiKen haiken)
    {
        hp = player.Hp;
        tension = player.Tension;
        stamina = player.Stamina;
        playerPos = player.playerPos.position.ToString();
    }
      
        

}

public class Save : MonoBehaviour, IInteraction
{
    SaveData saveData;
    Player player;
    HiRil hiril;
    HaiKen haiken;
    string path = "Assets/";
    string fileName = "SaveData.txt";

    public string InteractionText => "Save";


    private void Start()
    {
        player = GameManager.Instance.player;
        hiril= GameManager.Instance.hiril;
        haiken = GameManager.Instance.haiken;
    }

    public void SaveData()
    {
        StreamWriter sw;
        if(File.Exists(path+fileName) == false )
        {
            sw = File.CreateText(path+fileName);
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
        }
    }

    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.V))
        {
            LoadData();
            Debug.Log("로드");
        }
    }

    public void Active()
    {
        saveData = new SaveData(player, hiril, haiken);
        SaveData();
        Debug.Log("저장");
    }
}
