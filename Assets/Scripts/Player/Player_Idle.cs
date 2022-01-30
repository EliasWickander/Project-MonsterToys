using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Idle : State
{
    public override event Action<Enum> onStateTransition;
    
    public Player_Idle(Player controller) : base(controller.gameObject)
    {
    }
    
    public override void OnEnter(State prevState, object[] param)
    {
        
    }

    public override void OnTick()
    {
        
    }

    public override void OnExit(State nextState)
    {
        
    }
}
