using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes
{
    ChildRoom,
    CorridorRoom,
}

public class SceneStateMachine : MonoBehaviour
{
    private CameraFade m_cameraFade;
    
    public Dictionary<Scenes, SceneState> m_states;
    
    private SceneState m_currentState = null;
    public SceneState CurrentState => m_currentState;

    private Scenes m_currentStateEnum;
    public Scenes CurrentStateEnum => m_currentStateEnum;
    
    private ChildRoom m_childRoom;
    private CorridorRoom m_corridorRoom;

    public event Action onSceneTransitionStarted; 
    public event Action onSceneTransitionEnded;

    public event Action onSceneEnter;
    public event Action onSceneExit;

    private void Awake()
    {
        m_cameraFade = GetComponent<CameraFade>();
        
        m_childRoom = new ChildRoom();
        m_corridorRoom = new CorridorRoom();
        
        m_states = new Dictionary<Scenes, SceneState>()
        {
            {Scenes.ChildRoom, m_childRoom},
            {Scenes.CorridorRoom, m_corridorRoom}
        };
    }

    private void Start()
    {
        m_currentState = m_childRoom;
        m_currentStateEnum = Scenes.ChildRoom;
    }

    private void Update()
    {
        if (m_currentState != null)
        {
            m_currentState.OnTick();
        }
    }

    public void SetState(Scenes state)
    {
        StartCoroutine(SetState_Internal(state));
    }
    
    private IEnumerator SetState_Internal(Scenes state)
    {
        if (!m_states.ContainsKey(state))
            throw new Exception("Tried to set an invalid state");
        
        onSceneTransitionStarted?.Invoke();
        m_cameraFade.FadeOut();

        yield return new WaitUntil(() => m_cameraFade.CurrentState == CameraFade.FadeState.Idle);

        onSceneExit?.Invoke();

        SceneState oldState = m_currentState;
        SceneState newState = m_states[state];
        
        if (oldState != null)
            oldState.OnExit(newState);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(newState.SceneName);
        newState.OnEnter(oldState);
        m_currentState = newState;
        m_currentStateEnum = state;
        
        yield return new WaitUntil(() => asyncLoad.isDone);
        
        onSceneEnter?.Invoke();
        m_cameraFade.FadeIn();
        
        yield return new WaitUntil(() => m_cameraFade.CurrentState == CameraFade.FadeState.Idle);
        onSceneTransitionEnded?.Invoke();
    }
}
