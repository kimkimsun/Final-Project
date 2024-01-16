using CustomInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class SaveData
{

    public int hp;
    public int tension;
    public float stamina;
    public void Init()
    {
        hp = GameManager.Instance.player.Hp;
        tension = GameManager.Instance.player.Tension;
        stamina = GameManager.Instance.player.Stamina;
    }
      
        

}

public class Save : MonoBehaviour, IInteraction
{
    public SaveData saveData;

    string path = "Assets/";
    string fileName = "SaveData.txt";

    public string InteractionText => "Save";


    private void Start()
    {
        saveData = new SaveData();
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
        SaveData();
        Debug.Log("저장");
    }
}
