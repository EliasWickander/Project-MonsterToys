using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneState
{
    public abstract string SceneName { get; }
    public abstract void OnEnter(SceneState prevSceneState);
    public abstract void OnTick();
    public abstract void OnExit(SceneState nextSceneState);
}
