using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingleTon<SoundManager>
{

    [SerializeField] private SoundComponent SoundPrefab;
    private Queue<SoundComponent> pool = new Queue<SoundComponent>();


    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    private void Init()
    {
        for (int i = 0; i < 50; i++)
        {
            SoundComponent temp = Instantiate(SoundPrefab);
            temp.gameObject.SetActive(false);
            pool.Enqueue(temp);

        }
    }

    private SoundComponent Pop()
    {
        SoundComponent sound = pool.Dequeue();
        sound.gameObject.SetActive(true);
        return sound.GetComponent<SoundComponent>();
    }
    public void ReturnPool(SoundComponent sound)
    {
        sound.gameObject.SetActive(false);
        pool.Enqueue(sound);
    }

    public void Play(AudioClip clip, Transform target = null)
    {
        SoundComponent temp = Pop();
       // temp.transform.parent = target;
        temp.Play(clip);
    }

}