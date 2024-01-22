using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : Door
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<UseItem>().useItem_Type == USEITEM_TYPE.KEY)
        {
            StartCoroutine(OpenCo());
            SoundManager.Instance.Play(clip, false);
            GameManager.Instance.player.transform.position = nextPos.position;
            Destroy(gameObject);
        }
    }
}
