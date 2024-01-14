using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenesManager : SingleTon<ScenesManager> 
{
    public void DieScene()
    {
        Debug.Log("��������");
    }

    public void Exit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

}
