using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : SingleTon<UIManager>
{
    private Type pType;
    public Image openUI;
    public Image escapeCircle;
    public Image saveUI;
    public TextMeshProUGUI[] uiSettingText = new TextMeshProUGUI[3];
    public Animator tensionAni;
    public Animator hpAni;
    public GameObject settingBox;
    public AudioClip ClickSound;
    public UseItemInfo useItemInfo;
    public Stack<UnityEngine.Object> UIStack = new Stack<UnityEngine.Object>();
    public List<FieldInfo> uiViewFieldList;
}