using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalKey : MonoBehaviour, IInteraction
{
    public string InteractionText => "¿­¼è È¹µæ";

    public void Active()
    {
        GameManager.Instance.player.FinalKey++;
        Destroy(gameObject);
    }
}
