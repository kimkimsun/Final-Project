using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Aim : MonoBehaviour
{
    public TextMeshProUGUI text;
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward,out hit,100))
        {
            if (hit.transform.TryGetComponent<IActivable>(out IActivable hitResult))
                text.text = hit.transform.GetComponent<IActivable>().InteractionText;
        }
        else
            text.text = "";
    }
}