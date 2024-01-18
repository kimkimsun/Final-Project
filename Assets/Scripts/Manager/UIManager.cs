using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingleTon<UIManager>
{
    public Image openUI;
    public AudioClip ClickSound;
    public Image escapeCircle;
    public GameObject settingBox;
    public UseItemInfo useItemInfo;
    public Stack<Object> UIStack = new Stack<Object>();


    public void PlayClickSound()
    {
        SoundManager.Instance.Play(ClickSound);
    }
}
