using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStates
{
    Idle,
    Move,
}
public class Player : MonoBehaviour
{
    private RectTransform m_rectTransform;

    public Vector3 Position
    {
        get
        {
            return m_rectTransform.position;
        }
        set
        {
            m_rectTransform.position = value;
        }
    }

    public Vector3 LocalScale
    {
        get
        {
            return m_rectTransform.localScale;
        }
        set
        {
            m_rectTransform.localScale = value;
        }
    }

    private Animator m_animator;
    public Animator Animator => m_animator;

    private Player_Idle m_idleState;
    private Player_MoveTo m_moveState;
    
    private StateMachine m_stateMachine;
    
    public delegate void ReachedDestinationDelegate();

    private void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
        m_animator = GetComponent<Animator>();

        m_idleState = new Player_Idle(this);
        m_moveState = new Player_MoveTo(this);

        Dictionary<Enum, State> states = new Dictionary<Enum, State>()
        {
            {PlayerStates.Idle, m_idleState},
            {PlayerStates.Move, m_moveState}
        };

        m_stateMachine = new StateMachine(states);
    }

    private void Start()
    {
        SelectionManager.Instance.onObjectSelected += OnObjectSelected;
    }
    
    private void Update()
    {
        if (m_stateMachine != null)
        {
            m_stateMachine.Update();
        }
    }

    private void OnObjectSelected(Interactable selectedObject)
    {
        m_stateMachine.SetState(PlayerStates.Move, new object[] {selectedObject});
    }
    
    public void Flip(Vector3 dir)
    {
        Vector3 currentScale = LocalScale;
        
        if (Vector2.Dot(Vector2.right, dir) > 0)
        {
            currentScale.x = Mathf.Abs(currentScale.x);
        }
        else
        {
            currentScale.x = -Mathf.Abs(currentScale.x);
        }
        
        LocalScale = currentScale;
    }
}
