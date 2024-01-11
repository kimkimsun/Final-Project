using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionAim : MonoBehaviour
{
    private Vector3 screenCenter;
    public TextMeshProUGUI text;
    private int maxDistance;

    private void Start()
    {
        maxDistance = 1;
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
        }
        else
            text.text = "";
    }
}