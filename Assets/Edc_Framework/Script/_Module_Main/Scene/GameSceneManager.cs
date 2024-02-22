using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager
{
    public string currentSceneName;

    public GameSceneManager(){
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
        Hub.EventCenter.OnEvent(EventName.loadSceneBegin);
    }

    private void LoadSceneEnd(string sceneName){
        currentSceneName = sceneName;
        Hub.EventCenter.OnEvent(EventName.loadSceneEnd);
    }
}
