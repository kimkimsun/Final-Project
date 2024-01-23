using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopUpOff : MonoBehaviour
{
    public AudioClip clip;
    public GameObject popUp;
    public Button popOffButton;

    private void Start()
    {
        popOffButton = GetComponent<Button>();
        popOffButton.onClick.AddListener(() => { Off();}) ;
    }
     public void Off()
    {
        SoundManager.Instance.Play(clip,false);
        if (gameObject.name == "ExitButton")
            GameExit();
        else
            popUp.SetActive(false);
    }

    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
