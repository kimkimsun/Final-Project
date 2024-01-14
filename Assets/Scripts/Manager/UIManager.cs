using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingleTon<UIManager>
{
    public AudioClip ClickSound;

    public UseItemInfo useItemInfo;
    public GameObject settingBox;
    public Stack<Object> UIStack = new Stack<Object>();


    public void PlayClickSound()
    {
        SoundManager.Instance.Play(ClickSound);
    }
}
