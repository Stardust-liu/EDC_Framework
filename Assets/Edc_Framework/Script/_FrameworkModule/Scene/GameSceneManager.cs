using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameSceneEventName{
    public const string LoadSceneBegin = nameof(LoadSceneBegin);
    public const string LoadSceneEnd = nameof(LoadSceneBegin);
}

public class GameSceneManager
{
    public string currentSceneName;
    public static readonly EventCenter eventCenter = new EventCenter();

    public GameSceneManager(){
        SceneManager.sceneLoaded += LoadSceneEnd;
    }

    public void LoadScene(string sceneName){
        LoadSceneBegin();
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }   
    private void LoadSceneEnd(Scene scene, LoadSceneMode sceneMode){
        LoadSceneEnd(scene.name);
    }

    private void LoadSceneBegin(){
        eventCenter.OnEvent(GameSceneEventName.LoadSceneBegin);
    }

    private void LoadSceneEnd(string sceneName){
        currentSceneName = sceneName;
        eventCenter.OnEvent(GameSceneEventName.LoadSceneEnd);
    }
}
