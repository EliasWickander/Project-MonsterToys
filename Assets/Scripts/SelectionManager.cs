using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; private set; }

    private LevelManager m_levelManager = null;
    private Interactable m_selectedObject = null;

    public Interactable SelectedObject => m_selectedObject;

    public event Action<Interactable> onObjectSelected;
    private Player m_player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        m_levelManager = LevelManager.Instance;
        m_levelManager.onSceneEnter += OnSceneEnter;
        
        OnSceneEnter();
    }

    public void Select(Interactable interactable)
    {
        if (m_selectedObject != null)
            return;
        
        m_selectedObject = interactable;
        onObjectSelected?.Invoke(m_selectedObject);
    }

    public void OnReachedSelectedObject()
    {
        m_selectedObject.OnInteract();
        m_selectedObject = null;
    }

    private void OnSceneEnter()
    {
        Interactable[] allInteractables = FindObjectsOfType<Interactable>();

        foreach (Interactable interactable in allInteractables)
        {
            interactable.OnClick += Select;
        }
    }
}
