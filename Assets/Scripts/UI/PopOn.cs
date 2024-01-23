using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopUpOn : MonoBehaviour
{
    public AudioClip clip;
    public GameObject popUp;
    public Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(()=> { POPUp(); }) ;
    }

    public void POPUp()
    {
        SoundManager.Instance.Play(clip, false);
        popUp.SetActive(true);
    }

}
