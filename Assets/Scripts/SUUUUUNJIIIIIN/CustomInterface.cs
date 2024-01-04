using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace CustomInterface
{
    public interface IActivable
    {
        public Action Active();
    }

    public interface IStateMachine
    {
        public void SetState(string name);
        public object GetOwner();
    }
}