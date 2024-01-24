using CustomInterface;
using TMPro;
using UnityEngine;
using System;
public class InteractionAim : MonoBehaviour
{ 
    [SerializeField] private LayerMask monsterLayerMask;
    private Vector3 screenCenter;
    private int maxDistance;
    public TextMeshProUGUI text;
    public bool isLookMonster;
    private void Start()
    {
        maxDistance = 4;
        screenCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
    }
    private void FixedUpdate()
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
            else if (hit.transform.GetComponent<Monster>() != null)
            {
                isLookMonster = true;
            }
        }
        else
            text.text = "";
    }
/*    void Update()
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
            else if (hit.transform.GetComponent<Monster>() != null)
            {
                isLookMonster = true;
            }
        }
        else
            text.text = "";
    }*/
}