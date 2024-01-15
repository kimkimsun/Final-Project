using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class SaveData
{
    public float hp;
    public Transform monsterATrans;


    Player player = SaveManager.Instance.player;
    Monster monsterA = SaveManager.Instance.monsterA;
    Monster monsterB = SaveManager.Instance.monsterB;
}



public class SaveManager : SingleTon<SaveManager>
{
    public Player player;
    public Monster monsterA;
    public Monster monsterB;

    string path = "Assets/";
    string fileName = "SaveData.txt";
    public void SaveData()
    {
        StreamWriter sw;

        //Assets/SaveData.txt
        if (File.Exists(path + fileName) == false)
            sw = File.CreateText(path + fileName);
        else
            sw = new StreamWriter(path + fileName);

        sw.Write(JsonUtility.ToJson(player));
        sw.Close();
    }

    public void LoadData()
    {
        if (File.Exists(path + fileName))
        {
            StreamReader sr = new StreamReader(path + fileName);
            player = JsonUtility.FromJson<Player>(sr.ReadToEnd());
            sr.Close();
        }
    }
}
