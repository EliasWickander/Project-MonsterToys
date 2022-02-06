using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Cinematic
{
    public abstract CinematicScene[] Scenes { get; set; }

    private CinematicScene m_currentScene = null;
    private int m_currentSceneIndex = -1;
    
    public void Play()
    {
        m_currentSceneIndex = -1;
        StartNextScene();
        
    }

    public void Tick()
    {
        if (m_currentScene != null)
        {
            m_currentScene.Update();
        }
    }

    public void StartNextScene()
    {
        if (m_currentSceneIndex < Scenes.Length - 1)
        {
            if (m_currentScene != null)
            {
                m_currentScene.onFinished -= OnSceneFinished;
                m_currentScene.OnExit();
            }

            m_currentSceneIndex++;
            m_currentScene = Scenes[m_currentSceneIndex];
            m_currentScene.onFinished += OnSceneFinished;
            m_currentScene.OnEnter();
        }
        else
        {
            Debug.Log("All scenes finished");
        }
    }

    private void OnSceneFinished()
    {
        StartNextScene();
    }
}
