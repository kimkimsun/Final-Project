using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingleTon<UIManager>
{
    public Image openUI;
    public Image escapeCircle;
    public Animator tensionAni;
    public Animator hpAni;
    public GameObject settingBox;
    public AudioClip ClickSound;
    
    public UseItemInfo useItemInfo;

    public Stack<Object> UIStack = new Stack<Object>();
}

