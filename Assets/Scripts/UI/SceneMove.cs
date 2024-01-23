using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMove : MonoBehaviour
{
    public AudioClip clip;
    public string scenesname;
    public Button button;

    private void Start()
    {
        button.onClick.AddListener(() => 
        {
            SoundManager.Instance.Play(clip, false);
            SceneManager.LoadScene(scenesname); });
    }

}