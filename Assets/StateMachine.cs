using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IStateMachine
{
    void SetState(string name);
    object GetOwner();
}
public class State
{
    public IStateMachine sm = null;
    public event Action onEnter;

    public virtual void Init(IStateMachine sm)
    {
        this.sm = sm;
    }
    public virtual void Enter()
    {
        if (onEnter != null)
            onEnter();
    }
    public virtual void Update()
    {

    }
    public virtual void Exit()
    {

    }
}
public class StateMachine<T> : IStateMachine where T : class
{
    public T owner = null;
    public State curState = null;

    Dictionary<string, State> stateDic = null;
    public StateMachine()
    {
        stateDic = new Dictionary<string, State>();
    }
    public void AddState(string name, State state)
    {
        if (stateDic.ContainsKey(name))
            return;

        state.Init(this);
        stateDic.Add(name, state);
    }
    public object GetOwner()
    {
        return owner;
    }
    public void SetState(string name)
    {
        if (stateDic.ContainsKey(name))
        {
            if (curState != null)
                curState.Exit();
            curState = stateDic[name];
            curState.Enter();
        }
    }
    public void Update()
    {
        curState.Update();
    }
}