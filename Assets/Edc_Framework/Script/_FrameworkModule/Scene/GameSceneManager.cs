using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameSceneEventName{
    public const string LoadSceneBegin = nameof(LoadSceneBegin);
    public const string LoadSceneEnd = nameof(LoadSceneEnd);
}

public class GameSceneManager : BaseIOCComponent
{
    public string currentSceneName;
    public static readonly EventCenter eventCenter = new EventCenter();

    protected override void Init()
    {
        SceneManager.sceneLoaded += LoadSceneEnd;
    }

    public void LoadScene(string sceneName){
        LoadSceneBegin(sceneName);
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }   
    private void LoadSceneEnd(Scene scene, LoadSceneMode sceneMode){
        LoadSceneEnd(scene.name);
    }

    private void LoadSceneBegin(string sceneName){
        eventCenter.OnEvent(GameSceneEventName.LoadSceneBegin, sceneName);
    }

    private void LoadSceneEnd(string sceneName){
        currentSceneName = sceneName;
        eventCenter.OnEvent(GameSceneEventName.LoadSceneEnd, sceneName);
    }
}
