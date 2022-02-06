using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicTester : MonoBehaviour
{
    private CinematicMaster m_cinematicMaster;

    private void Awake()
    {
        m_cinematicMaster = GetComponent<CinematicMaster>();
    }

    private void Start()
    {
        m_cinematicMaster.StartCinematic(new TestCinematic());
    }
}
