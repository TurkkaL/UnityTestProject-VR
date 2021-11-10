using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // needed for Events/Actions


// Enums are nice! Make a list of states or options.
public enum SceneState
{
    Menu,
    Running,
    Failed,
    Won
}

public class SceneStateManager : MonoBehaviour
{
    static public SceneStateManager Instance;
    static public event Action<SceneState> OnSceneStateChanged;
    public SceneState sceneState;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        ChangeState(sceneState);
    }

    public void ChangeState(SceneState setState)
    {
        sceneState = setState;
        // Shout out the event that sceneState has been changed
        // (into setState)
        if (OnSceneStateChanged != null) OnSceneStateChanged(setState);

        if (setState == SceneState.Failed)
        {
            ReloadScene();
        }
    }

    private void ReloadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(5);
    }
}