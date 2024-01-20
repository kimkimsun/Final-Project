using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PopUpOff : MonoBehaviour, IPointerClickHandler
{
    public AudioClip clip;
    public GameObject popUp;
    public void OnPointerClick(PointerEventData eventData)
    {
        SoundManager.Instance.Play(clip);
        if (gameObject.name == "ExitButton")
            GameExit();
        else
            SetOffPopUp(popUp);
    }
    public void SetOffPopUp(GameObject popUp)
    {
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
