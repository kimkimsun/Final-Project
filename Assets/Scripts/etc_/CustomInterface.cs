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

    public interface ISubscribeable
    {
        public void OnEvent();

    }

    public interface IEventable
    {
        public void Raise();
        public void RegisterListener(ISubscribeable listener);
        public void UnregisterListener(ISubscribeable listener);
    }
    public interface IStunable
    {
        public void Stun();
    }
}