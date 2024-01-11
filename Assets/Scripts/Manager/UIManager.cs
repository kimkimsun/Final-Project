using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingleTon<UIManager>
{
    public UseItemInfo useItemInfo;
    public Stack<Object> UIStack = new Stack<Object>();
    public GameObject settingBox;
}
