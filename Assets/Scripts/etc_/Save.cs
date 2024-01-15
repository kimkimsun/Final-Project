using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class Save : MonoBehaviour
{
    Player player;

    string path = "Assets/";
    string fileName = "SaveData.txt";

    private void Start()
    {
        player = GameManager.Instance.player;
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
        sw.Write(JsonUtility.ToJson(player));
        sw.Close();
    }

    public void LoadData()
    {
        if(File.Exists(path+fileName))
        {
            StreamReader sr =new StreamReader(path+fileName);
            player = JsonUtility.FromJson<Player>(sr.ReadToEnd());
            sr.Close();
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L)) 
        {
            SaveData();
        }
        
        if(Input.GetKeyDown(KeyCode.K))
        {
            LoadData();
        }
    }
}
