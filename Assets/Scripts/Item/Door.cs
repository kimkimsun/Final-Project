using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour, IInteraction
{

    public string InteractionText => "open";
    public Transform nextPos;
    public Transform prevPos;
    private bool isOpen;
 
    private void Start()
    {
        isOpen = false;
    }
    public bool IsOpen
    {

        get => isOpen;
        set
        {
            isOpen = value;
            if( isOpen )
            {   
                 GameManager.Instance.player.transform.position = nextPos.position;
                 StartCoroutine(OpenCo());
            }
            else
            {
                if (prevPos == null)
                {
                    //´úÄÈÀÌ´Â »ç¿îµå
                    return;
                }
                else
                {
                    GameManager.Instance.player.transform.position = prevPos.position;
                    StartCoroutine(OpenCo());
                }
            }
        }


    }


    public void Active()
    {
        IsOpen = !IsOpen;
    }

    IEnumerator OpenCo()
    {
        float alpha = 1;
        UIManager.Instance.openUI.color = new Color(0,0,0,0.8f);
        while (alpha >= 0)
        {
            UIManager.Instance.openUI.color = new Color(0, 0, 0, alpha);
            yield return new WaitForSeconds(0.2f);
            alpha -= 0.1f;
        }

    }

}
