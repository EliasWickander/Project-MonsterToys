using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine
{
    public Dictionary<Enum, State> m_states;

    private State m_currentState = null;
    public State CurrentState => m_currentState;

    public StateMachine(Dictionary<Enum, State> states)
    {
        this.m_states = states;
        
        SetState(m_states.Keys.First());
    }

    public void Update()
    {
        if (m_currentState != null)
        {
            m_currentState.OnTick();
        }
    }

    public void SetState(Enum state, object[] param)
    {
        if (!m_states.ContainsKey(state))
            throw new Exception("Tried to set an invalid state");
        
        State oldState = m_currentState;
        State newState = m_states[state];

        if (oldState != null)
        {
            oldState.onStateTransition -= SetState;
            oldState.OnExit(newState);   
        }

        newState.onStateTransition += SetState;
        newState.OnEnter(oldState, param);
        m_currentState = newState;
    }

    public void SetState(Enum state)
    {
        SetState(state, null);
    }
}
