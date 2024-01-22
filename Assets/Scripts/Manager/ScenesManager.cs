using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : SingleTon<ScenesManager>
{
    public int loadSceneindex = 1;
    public GameObject UICanvas;
    public CinemachineVirtualCamera EndVC;
    public Light EndLight;

    private float time;
    public void DieScene()
    {
        UICanvas.gameObject.SetActive(false);
        EndVC.Priority = 11;
        StartCoroutine(CameraMove());
    }

    IEnumerator CameraMove()
    {
        time = 0;
        UIManager.Instance.openUI.color = new Color(0, 0, 0, 1);
        while (time <= 2.5f)
        {
            yield return null;
            time += Time.deltaTime;
        }
        StartCoroutine(EndProduce());
    }
    IEnumerator EndProduce()
    {
        float alpha = 1f;
        EndLight.intensity = 10;
        while (alpha >= 0)
        {
            UIManager.Instance.openUI.color = new Color(0, 0, 0, alpha);
            yield return new WaitForSeconds(0.1f);
            alpha -= 0.05f;
        }
        while (alpha <= 1)
        {
            UIManager.Instance.openUI.color = new Color(0, 0, 0, alpha);
            alpha += 0.05f;
            EndLight.intensity -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
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
