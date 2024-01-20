using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PopUpOn : MonoBehaviour, IPointerClickHandler
{
    public AudioClip clip;
    public GameObject popUp;
    public void OnPointerClick(PointerEventData eventData)
    {
        SoundManager.Instance.Play(clip,false);
        SetOnPopUp(popUp);
    }

    public void SetOnPopUp(GameObject popUpName)
    {
        popUp.SetActive(true);
    }
}
