using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionAim : MonoBehaviour
{
    private Vector3 screenCenter;
    public TextMeshProUGUI text;

    private void Start()
    {
        screenCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
    }
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10))
        {

            if (hit.transform.TryGetComponent<IInteraction>(out IInteraction hitResult))
            {
                text.text = hit.transform.GetComponent<IInteraction>().InteractionText;
                if (Input.GetMouseButtonDown(0))
                    hit.transform.GetComponent<IInteraction>().Active();
            }
            else
                text.text = "";
        }
    }
}