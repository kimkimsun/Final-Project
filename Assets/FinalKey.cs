using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalKey : MonoBehaviour, IInteraction
{
    public string InteractionText => "���� ȹ��";

    public void Active()
    {
        GameManager.Instance.player.FinalKey++;
        Destroy(gameObject);
    }
}
