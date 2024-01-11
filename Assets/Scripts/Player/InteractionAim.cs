using CustomInterface;
using TMPro;
using UnityEngine;
using System;
public class InteractionAim : MonoBehaviour
{
    [SerializeField] private LayerMask monsterLayerMask;
    public Action monsterCheck;
    private Vector3 screenCenter;
    public TextMeshProUGUI text;
    private int maxDistance;
    private void Start()
    {
        maxDistance = 15;
        screenCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
    }
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if (hit.transform.TryGetComponent<IInteraction>(out IInteraction hitResult))
            {
                text.text = hitResult.InteractionText;
                if (Input.GetMouseButtonDown(0))
                {
                    hitResult.Active();
                }
            }
            if (hit.transform.TryGetComponent<Monster>(out Monster mon))
            {
                Debug.Log("°¨ÁöÇÔ");
                monsterCheck();
            }
            else
                return;
        }
        else
            text.text = "";
    }
}