using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour, IInteraction
{

    public string InteractionText => "open";
    public AudioClip clip;
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
                 StartCoroutine(OpenCo());
                SoundManager.Instance.Play(clip, false);
                GameManager.Instance.player.transform.position = nextPos.position;
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
                    StartCoroutine(OpenCo());
                    SoundManager.Instance.Play(clip, false);
                    GameManager.Instance.player.transform.position = prevPos.position;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Monster>() != null)
            GetComponent<BoxCollider>().enabled = true;
        else if(collision.gameObject.GetComponent<Monster>() != null)
            GetComponent<BoxCollider>().enabled = false;
    }

    public void Active()
    {
        IsOpen = !IsOpen;
    }

    protected IEnumerator OpenCo()
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
