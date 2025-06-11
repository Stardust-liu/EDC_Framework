using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : BaseIOCComponent, ISendEvent
{
    public string currentSceneName;

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
        this.SendEvent(new LoadSceneBegin(sceneName));
    }

    private void LoadSceneEnd(string sceneName){
        currentSceneName = sceneName;
        this.SendEvent(new LoadSceneEnd(sceneName));
    }
}
