using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CustomInterface
{
    public enum MASTATE_TYPE
    {
        abc,
    }
    public interface IStateMachine
    {
        public void SetState(string name);
        public object GetOwner();
    }
}