using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    private Player m_player = null;
    public Player Player => m_player;

    private Pathfinding m_pathFinding = null;
    public Pathfinding PathFinding => m_pathFinding;

    private SceneStateMachine m_sceneStateMachine;

    private Scenes m_prevScene;

    public event Action onSceneEnter;
    public event Action onSceneExit;
    
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
        
        m_player = FindObjectOfType<Player>();
        m_sceneStateMachine = GetComponent<SceneStateMachine>();

        m_pathFinding = FindObjectOfType<Pathfinding>();

    }

    private void Start()
    {
        m_sceneStateMachine.onSceneTransitionStarted += () => GameManager.Instance.IsPaused = true;
        m_sceneStateMachine.onSceneTransitionEnded += () => GameManager.Instance.IsPaused = false;
        
        m_sceneStateMachine.onSceneEnter += () => onSceneEnter?.Invoke();
        m_sceneStateMachine.onSceneExit += () => onSceneExit?.Invoke();

        onSceneEnter += OnSceneEnter;
        onSceneExit += OnSceneExit;
    }

    public void ChangeScene(Scenes scene)
    {
        m_sceneStateMachine.SetState(scene);
    }

    private void OnSceneExit()
    {
        m_prevScene = m_sceneStateMachine.CurrentStateEnum;
    }
    
    private void OnSceneEnter()
    {
        m_pathFinding = FindObjectOfType<Pathfinding>();
        Door[] allDoors = FindObjectsOfType<Door>();

        foreach (Door door in allDoors)
        {
            if (door.SceneToLoad == m_prevScene)
            {
                m_player.Position = door.InteractionPoint;
                m_player.Flip(door.Normal);
                break;
            }
        }
    }
}
