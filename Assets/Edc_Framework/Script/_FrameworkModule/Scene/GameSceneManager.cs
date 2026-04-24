using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : BaseIOCComponent, ISendEvent
{
    public string currentSceneName;
    private SceneResourceConfig lastSceneResourceConfig;
    private SceneResourcesSetting SceneResourcesSetting { get; set; }

    protected override void Init()
    {
        base.Init();
        SceneResourcesSetting = Hub.Resources.Get<SceneResourcesSetting>("SceneResourcesSetting");
    }

    public async UniTask LoadScene(string sceneName){
        LoadSceneBegin(sceneName);
        var sceneOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        sceneOperation.allowSceneActivation = false;
        await ResourcesHandle(sceneName);
        await UniTask.WaitUntil(() => sceneOperation.progress >= 0.9f);
        sceneOperation.allowSceneActivation = true;
        await sceneOperation.ToUniTask();
        LoadSceneEnd(sceneName);
    }   

    private void LoadSceneBegin(string sceneName){
        this.SendEvent(new LoadSceneBegin(sceneName));
    }

    private async UniTask ResourcesHandle(string sceneName)
    {
        var resourceConfig = SceneResourcesSetting.GetResourceConfig(sceneName);
        if(resourceConfig != null)
        {
            await resourceConfig.Load();
        }
        if(lastSceneResourceConfig != null && lastSceneResourceConfig.isChangeSceneAutoRelease)
        {
            lastSceneResourceConfig.Release();
        }
        lastSceneResourceConfig = resourceConfig;
    }

    private void LoadSceneEnd(string sceneName){
        currentSceneName = sceneName;
        this.SendEvent(new LoadSceneEnd(sceneName));
    }
}
