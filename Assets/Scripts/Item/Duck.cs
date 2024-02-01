using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour, ISoundable
{
    public float Sound => 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            Active();
            transform.GetChild(0).GetComponent<CapsuleCollider>().enabled = true;
        }
    }
    public void Active()
    {
        StartCoroutine(UIManager.Instance.SoundCo(Sound));
    }
}
