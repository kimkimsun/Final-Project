using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class Save : MonoBehaviour, IInteraction
{
    Player player;

    string path = "Assets/";
    string fileName = "SaveData.txt";

    public string InteractionText => "Save";

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
        
        if(Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("로드");
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
