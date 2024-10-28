using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class ABManager
{
    private readonly Dictionary<string, AssetBundle> abFile = new ();
    private AssetBundleManifest abManifest;

    private string MainAbName{
        get{
#if UNITY_ANDROID
            return "Android";
#elif UNITY_IOS
            return "IOS";
#else
            return "StandaloneWindows";
#endif
        }
    }

    public ABManager(){
        var ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath+$"/{MainAbName}");
        abManifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
    }

    /// <summary>
    /// 同步加载ab包文件
    /// </summary>
    public void LoadAbFile(string fileNmae){
        if(abFile.ContainsKey(fileNmae)){
            LogManager.LogWarning("重复加载");
            return;
        }
        var path = Application.streamingAssetsPath+$"/{fileNmae}";
        if (!File.Exists(path))
        {
            Debug.LogWarning($"没有AB包文件：{fileNmae}");
        }
        else{
            var ab = AssetBundle.LoadFromFile(path);
            var dependencies = abManifest.GetAllDependencies(fileNmae);
            AssetBundle dependenciesAb;
            foreach (var item in dependencies)
            {
                if(!abFile.ContainsKey(item)){
                    dependenciesAb = AssetBundle.LoadFromFile(Application.streamingAssetsPath+$"/{item}");
                    abFile.Add(fileNmae,dependenciesAb);
                }
            }
            abFile.Add(fileNmae,ab);
        }
    }

    /// <summary>
    /// 异步加载ab包文件
    /// </summary>
    public void LoadAbFileAsync(string fileNmae, Action callback = null){
         if(abFile.ContainsKey(fileNmae)){
            LogManager.LogWarning("重复加载");
            return;
        }
        Hub.Coroutine.StartCoroutine(LoadAbFileAsyncCoroutine());
        IEnumerator LoadAbFileAsyncCoroutine(){
            var ab = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath+$"/{fileNmae}");
            var dependencies = abManifest.GetAllDependencies(fileNmae);
            AssetBundleCreateRequest dependenciesAb;
            foreach (var item in dependencies)
            {
                if(!abFile.ContainsKey(item)){
                    dependenciesAb = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath+$"/{item}");
                    yield return dependenciesAb;
                    abFile.Add(fileNmae, dependenciesAb.assetBundle);
                }
            }
            yield return ab;

            abFile.Add(fileNmae, ab.assetBundle);
            callback?.Invoke();
        }
    }

    /// <summary>
    /// 同步加载ab包对象
    /// </summary>
    public T LoadAsset<T>(string abName, string fileNmae) where T : class{
        return abFile[abName].LoadAsset(fileNmae) as T;
    }

    /// <summary>
    /// 异步加载ab包对象
    /// </summary>
    public void LoadAssetAsync<T>(string abName, string fileNmae, Action<T> callback = null) where T : class{
        Hub.Coroutine.StartCoroutine(LoadAssetAsyncCoroutine());
        IEnumerator LoadAssetAsyncCoroutine(){
            var ab = abFile[abName].LoadAssetAsync<GameObject>(fileNmae).asset as T;
            yield return ab;
            callback?.Invoke(ab);
        }
    }

    /// <summary>
    /// 同步卸载指定ab包
    /// </summary>
    public void UnloadAb(string fileNmae, bool unloadAllLoadedObjects){
        abFile[fileNmae].Unload(unloadAllLoadedObjects);
        abFile.Remove(fileNmae);
    }

    /// <summary>
    /// 异步卸载指定ab包
    /// </summary>
    public void UnloadAsync(string fileNmae, bool unloadAllLoadedObjects, Action callback = null){
        Hub.Coroutine.StartCoroutine(UnloadAsyncCoroutine());
        IEnumerator UnloadAsyncCoroutine(){
            yield return abFile[fileNmae].UnloadAsync(unloadAllLoadedObjects);
            abFile.Remove(fileNmae);
            callback?.Invoke();
        }
    }

    /// <summary>
    /// 卸载所有ab包
    /// </summary>
    public void UnloadAllAb(bool unloadAllLoadedObjects){
        AssetBundle.UnloadAllAssetBundles(unloadAllLoadedObjects);
        abFile.Clear();
    }
}
