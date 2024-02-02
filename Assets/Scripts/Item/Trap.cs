using CustomInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TrapStrategy
{
    public Trap owner;
    public TrapStrategy(Trap owner)
    { this.owner = owner; }
    public abstract void Use();
}

public class DuckTrapStrategy : TrapStrategy
{
    public DuckTrapStrategy(Trap owner) : base(owner) { this.owner = owner; }

    public override void Use()
    {
        throw new System.NotImplementedException();
    }
}
public class SurpriseBoxTrapStrategy : TrapStrategy
{
    public SurpriseBoxTrapStrategy(Trap owner) : base(owner) { this.owner = owner; }

    public override void Use()
    {
        throw new System.NotImplementedException();
    }
}
public class BearTrapStrategy : TrapStrategy
{
    public BearTrapStrategy(Trap owner) : base(owner) { this.owner = owner; }

    public override void Use()
    {
        throw new System.NotImplementedException();
    }
}
public enum TRAP_TYPE
{
    DUCK,
    SURPRISEBOX,
    BEARTRAP,
}

public class Trap : MonoBehaviour, ISoundable
{
    [SerializeField] private TRAP_TYPE trapType;
    private TrapStrategy trapStrategy;
    private float sound;
    public float Sound => sound;

    private void Start()
    {
        switch (trapType)
        {
            case TRAP_TYPE.DUCK:
                trapStrategy = new DuckTrapStrategy(this);
                sound = 3f;
                break;
            case TRAP_TYPE.SURPRISEBOX:
                trapStrategy = new SurpriseBoxTrapStrategy(this);
                sound = 5;
                break;
            case TRAP_TYPE.BEARTRAP:
                trapStrategy = new BearTrapStrategy(this);
                sound = 10;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            Player player = other.GetComponent<Player>();
            transform.GetChild(0).GetComponent<CapsuleCollider>().enabled = true;
        }
    }
    public void Active()
    {
        StartCoroutine(UIManager.Instance.SoundCo(Sound));
    }
}
