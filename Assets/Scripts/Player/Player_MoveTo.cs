using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_MoveTo : State
{
    public override event Action<Enum> onStateTransition;
    
    private SelectionManager m_selectionManager;

    private Player m_controller;

    private Interactable m_targetObject = null;
    private Vector3 m_dirToDest = Vector3.zero;
    public Player_MoveTo(Player controller) : base(controller.gameObject)
    {
        this.m_controller = controller;
    }
    

    public override void OnEnter(State prevState, object[] param)
    {
        m_selectionManager = SelectionManager.Instance;
        
        m_targetObject = param[0] as Interactable;
        
        m_dirToDest = m_targetObject.InteractionPoint - m_controller.Position;
        m_dirToDest.Normalize();
        
        m_controller.Flip(m_dirToDest);
        
        m_controller.Animator.SetBool("IsWalking", true);
    }

    public override void OnTick()
    {
        if (Vector3.Distance(m_controller.Position, m_targetObject.InteractionPoint) > 50f)
        {
            m_controller.Position += m_dirToDest * 200 * Time.deltaTime;
        }
        else
        {
            onStateTransition?.Invoke(PlayerStates.Idle);
        }
    }

    public override void OnExit(State nextState)
    {
        m_controller.Animator.SetBool("IsWalking", false);
        
        m_selectionManager.OnReachedSelectedObject();
        
        m_targetObject = null;
        m_dirToDest = Vector3.zero;
    }
}
