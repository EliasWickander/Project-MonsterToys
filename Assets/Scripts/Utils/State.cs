using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    private GameObject m_controller;

    public abstract event Action<Enum> onStateTransition;
    
    public State(GameObject controller)
    {
        this.m_controller = controller;
    }
    
    public abstract void OnEnter(State prevState, object[] param);
    public abstract void OnTick();
    public abstract void OnExit(State nextState);
}
