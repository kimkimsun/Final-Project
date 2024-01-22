using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour, IPointerClickHandler
{
    public AudioClip clip;
    public int loadDataIndex;
    public string scenesname;
    public void OnPointerClick(PointerEventData eventData)
    {
        //SoundManager.instance.SFXPlay("Button", clip);
        GameScenes(scenesname);
    }

    public void GameScenes(string scenesname)
    {
        SceneManager.LoadScene(scenesname);
    }
}