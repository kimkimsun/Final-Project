using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour, IInteraction
{
    public Image openUI;
    private Color darkColor;
    public Transform nextPos;
    public Transform prevPos;
    private bool isOpen;

    private void Start()
    {
//        darkColor = openUI.color;
        isOpen = false;
    }
    public bool IsOpen
    {

        get => isOpen;
        set
        {
            isOpen = value;
            if( isOpen )
                GameManager.Instance.player.transform.position = nextPos.position;
            else
                GameManager.Instance.player.transform.position = prevPos.position;
        }


    }

    public string InteractionText => "Open";

    public void Active()
    {
        //StartCoroutine(OpenCo());
        IsOpen = !IsOpen;
    }

/*    IEnumerator OpenCo()
    {
        Debug.Log("코루틴 들어옴");
        darkColor = new Color(0, 0, 0, 1);
        while (darkColor.a >= 0) 
        {
            darkColor.a -= 0.1f;            
            yield return new WaitForSeconds(0.5f);

        }

    }*/

}
