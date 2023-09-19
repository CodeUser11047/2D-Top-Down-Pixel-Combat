using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagment : Singleton<SceneManagment>
{
    public event EventHandler OnExitScene;
    protected override void Awake()
    {
        base.Awake();
    }

    public void OnExitSceneEvent()
    {
        OnExitScene?.Invoke(this, EventArgs.Empty);
    }

    public string SceneTransitionName { get; private set; }
    public void SetTransitionName(string sceneTransitionName)
    {
        this.SceneTransitionName = sceneTransitionName;
    }

}
