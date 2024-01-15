using CustomInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterState : State
{
    public Monster owner;
    public IEnumerator monsterMoveCo;
    public override void Init(IStateMachine sm) { }
    public override void Enter() { }
    public override void Update() { }
    public override void Exit() { }
}