using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CinematicScene
{
    public abstract event Action onFinished;
    public abstract void OnEnter();
    public abstract void Update();
    public abstract void OnExit();
}
