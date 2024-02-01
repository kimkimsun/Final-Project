using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : SingleTon<UIManager>
{
    public Image openUI;
    public Image escapeCircle;
    public Image saveUI;
    public Image loadImage;
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

    private float alpha;
    private float time;
    private bool check;
    private void Start()
    {
        alpha = 0.4f;
        StartCoroutine(ImageBlinkPlusCo());
    }

    public IEnumerator SoundCo(float sound)
    {
        Debug.Log("µé¾î¿È?");
        float scrollSound = 0;
        while(scrollSound <= (sound/2))
        {
            yield return null;
            scrollSound += Time.deltaTime;
            soundScroll.value = scrollSound / (sound * 10);
        }
        while(scrollSound >= 0)
        {
            yield return null;
            scrollSound -= Time.deltaTime;
            soundScroll.value = scrollSound / (sound * 10);
        }
    }

    IEnumerator ImageBlinkPlusCo()
    {
        while (alpha <= 1.0f)
        {
            for (int i = 0; i < carImage.Length; i++)
            {
                if (check |= carImage[i].gameObject.activeSelf == false)
                    StopAllCoroutines();
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
        while (alpha >= 0.4f)
        {
            for (int i = 0; i < carImage.Length; i++)
            {
                if (check |= carImage[i].gameObject.activeSelf == false)
                    StopAllCoroutines();
                if (carImage[i].gameObject.activeSelf)
                {
                    carImage[i].color = new Color(1, 1, 1, alpha);
                    yield return new WaitForSeconds(0.01f);
                    alpha -= 0.0119f;
                    Debug.Log("¹Ø¿¡°Å");
                }
            }
        }
        StartCoroutine(ImageBlinkPlusCo());
    }
}