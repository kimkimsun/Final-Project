using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : SingleTon<UIManager>
{
    public Image escapeCircle;
    public Image saveUI;
    public Image[] carImage = new Image[5];
    public Scrollbar soundScroll;
    public Button loadButton;
    public Button saveButton;
    public TextMeshProUGUI uiSettingText;
    public Animator tensionAni;
    public Animator hpAni;
    public GameObject settingBox;
    public AudioClip ClickSound;
    public UseItemInfo useItemInfo;
    public Stack<UnityEngine.Object> UIStack = new Stack<UnityEngine.Object>();
    public List<FieldInfo> uiViewFieldList;
    public IEnumerator soundCo;

    private float alpha;
    private float tempSound;
    private bool check;
    private void Start()
    {
        alpha = 0.4f;
        StartCoroutine(ImageBlinkPlusCo());
    }
    public void SaveSoundCo(float sound)
    {
        if (tempSound < sound)
        {
            StopCoroutine(soundCo);
            tempSound = sound;
            soundCo = SoundCo(sound);
            StartCoroutine(soundCo);
        }
        else
            return;
    }

    public IEnumerator SoundCo(float sound)
    {
        float scrollSound = 0;
        while (scrollSound <= (sound / 2))
        {
            yield return null;
            scrollSound += Time.deltaTime;
            soundScroll.value = scrollSound / 10;
        }
        while (scrollSound >= 0)
        {
            yield return null;
            scrollSound -= Time.deltaTime;
            soundScroll.value = scrollSound / 10;
        }
        tempSound = 0;
    }

    IEnumerator ImageBlinkPlusCo()
    {
        check = false;
        foreach (Image image in carImage)
        {
            check &= !(image.gameObject.activeSelf);
        }
        if (check)
            yield break;
        while (alpha <= 1.0f)
        {
            for (int i = 0; i < carImage.Length; i++)
            {
                if (carImage[i].gameObject.activeSelf)
                {
                    carImage[i].color = new Color(1, 1, 1, alpha);
                    yield return new WaitForSeconds(0.01f);
                    alpha += 0.0119f;
                }
            }
        }
        StartCoroutine(ImageBlinkMinusCo());
    }
    IEnumerator ImageBlinkMinusCo()
    {
        check = false;
        foreach (Image image in carImage)
        {
            check &= !(image.gameObject.activeSelf);
        }
        if (check)
            yield break;
        while (alpha >= 0.4f)
        {
            for (int i = 0; i < carImage.Length; i++)
            {
                if (carImage[i].gameObject.activeSelf)
                {
                    carImage[i].color = new Color(1, 1, 1, alpha);
                    yield return new WaitForSeconds(0.01f);
                    alpha -= 0.0119f;
                }
            }
        }
        StartCoroutine(ImageBlinkPlusCo());
    }
}