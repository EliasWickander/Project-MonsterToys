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

    private Pathfinding m_pathFinding = null;
    private List<NavNode> m_path;
    private int m_nodeIndex = 0;
    
    public Player_MoveTo(Player controller) : base(controller.gameObject)
    {
        this.m_controller = controller;
    }
    

    public override void OnEnter(State prevState, object[] param)
    {
        m_selectionManager = SelectionManager.Instance;
        m_pathFinding = LevelManager.Instance.PathFinding;
        m_targetObject = param[0] as Interactable;

        if (m_targetObject == null)
        {
            Debug.LogError("Target object is null. Likely because it hasn't been set as a parameter");
            return;
        }
        
        m_path = m_pathFinding.FindPath(m_controller.Position, m_targetObject.InteractionPoint);

        if (m_path == null)
            return;
        
        FlipToCurrentDir();
        
        m_controller.Animator.SetBool("IsWalking", true);
    }

    public override void OnTick()
    {
        Vector3 dirToNode = m_path[m_nodeIndex].m_worldPos - m_controller.Position;

        if (dirToNode.magnitude > 0.2f)
        {
            dirToNode.Normalize();
            m_controller.Position += dirToNode * 200 * Time.deltaTime;
        }
        else
        {
            m_nodeIndex++;

            if (m_nodeIndex < m_path.Count)
            {
                FlipToCurrentDir();
            }
            else
            {
                onStateTransition?.Invoke(PlayerStates.Idle);
                return;
            }
        }
        
        DebugPath();
    }

    public override void OnExit(State nextState)
    {
        m_controller.Animator.SetBool("IsWalking", false);
        
        m_selectionManager.OnReachedSelectedObject();
        
        m_targetObject = null;
        m_path = null;
        m_nodeIndex = 0;
    }
    
    private void FlipToCurrentDir()
    {
        Vector3 dirToNode = m_path[m_nodeIndex].m_worldPos - m_controller.Position;
        dirToNode.Normalize();
        
        m_controller.Flip(dirToNode);
    }

    private void DebugPath()
    {
        if (m_path == null)
            return;
        
        for (int i = 0; i < m_path.Count; i++)
        {
            if (i < m_path.Count - 1)
            {
                NavNode currentNode = m_path[i];
                NavNode nextNode = m_path[i + 1];
                Debug.DrawLine(currentNode.m_worldPos, nextNode.m_worldPos, Color.green);
            }
                
        }
    }
}
