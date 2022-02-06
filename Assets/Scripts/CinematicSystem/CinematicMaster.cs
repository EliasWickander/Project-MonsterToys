using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicMaster : MonoBehaviour
{
    private Cinematic m_currentCinematic = null;

    
    public void StartCinematic(Cinematic cinematic)
    {
        if (cinematic.Scenes.Length <= 0)
        {
            Debug.LogError("Trying to start cinematic with scene count 0. Aborting...");
            return;
        }

        m_currentCinematic = cinematic;
        m_currentCinematic.Play();
    }

    private void Update()
    {
        if (m_currentCinematic != null)
        {
            m_currentCinematic.Tick();
        }
    }
}
