using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace CustomInterface
{
    public interface IInteraction
    {
        public string InteractionText
        {
            get;
        }
        public void Active();
    }
    public interface IStateMachine
    {
        public void SetState(string name);
        public object GetOwner();
    }

    public interface IStunable
    {
        public void Stun();
    }
}