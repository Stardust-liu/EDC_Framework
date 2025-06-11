using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ResourcesModule : BaseIOCComponent
{
    private ABManager aBManager;
    
    protected override void Init()
    {
        aBManager = new ABManager();
        LoadAbFile("scriptableobject");
        LoadAbFile("soundBg");
        LoadAbFile("soundEffect");
        LoadAbFile("soundDialogue");
        LoadAbFile("csv");
        LoadAbFile("sprite");
        LoadAbFile("prefab");
    }
    
    /// <summary>
    /// 卸载资源
    /// </summary>
    private void UnloadResources(){
        Resources.UnloadUnusedAssets();
    }
#region AB包加载
    /// <summary>
    /// 同步加载ab包对象
    /// </summary>
    public void LoadAbFile(string fileNmae){
        aBManager.LoadAbFile(fileNmae);
    }

    /// <summary>
    /// 异步加载ab包文件
    /// </summary>
    public void LoadAbFileAsync(string fileNmae, Action callback = null){
        aBManager.LoadAbFileAsync(fileNmae, callback);
    }

    /// <summary>
    /// 同步卸载指定ab包
    /// </summary>
    public void UnloadAb(string fileNmae, bool unloadAllLoadedObjects){
        aBManager.UnloadAb(fileNmae, unloadAllLoadedObjects);
    }

    /// <summary>
    /// 异步卸载指定ab包
    /// </summary>
    public void UnloadAsync(string fileNmae, bool unloadAllLoadedObjects, Action callback){
        aBManager.UnloadAsync(fileNmae, unloadAllLoadedObjects, callback);
    }

    /// <summary>
    /// 卸载所有ab包
    /// </summary>
    public void UnloadAllAb(bool unloadAllLoadedObjects){
        aBManager.UnloadAllAb(unloadAllLoadedObjects);
    }

    /// <summary>
    /// 同步加载ab包对象
    /// </summary>
    public T LoadAsset<T>(string abName, string fileNmae) where T : class{
        return aBManager.LoadAsset<T>(abName, fileNmae);
    }

    /// <summary>
    /// 异步加载ab包对象
    /// </summary>
    public void LoadAssetAsync<T>(string abName, string fileNmae, Action<T> callback = null) where T : class{
        aBManager.LoadAssetAsync<T>(abName, fileNmae, callback);
    }
#endregion
#region 脚本化对象
    /// <summary>
    /// 获取脚本化对象
    /// </summary>
    public T GetScriptableobject<T>(ResourcePathAttribute resourcePath) where T : ScriptableObject{
        return Get<T>("scriptableobject", resourcePath);
    }

    /// <summary>
    /// 获取脚本化对象
    /// </summary>
    public T GetScriptableobject<T>(ResourcePath resourcePath) where T : ScriptableObject{
        return Get<T>("scriptableobject", resourcePath);
    }
#endregion
#region 音频文件
    /// <summary>
    /// 获取背景音
    /// </summary>
    public AudioClip GetSoundBg(ResourcePathAttribute resourcePath){
        return Get<AudioClip>("soundBg", resourcePath);
    }

    /// <summary>
    /// 获取背景音
    /// </summary>
    public AudioClip GetSoundBg(ResourcePath resourcePath){
        return Get<AudioClip>("soundBg", resourcePath);
    }

    /// <summary>
    /// 获取音效
    /// </summary>
    public AudioClip GetSoundEffect(ResourcePath resourcePath){
        return Get<AudioClip>("soundEffect", resourcePath);
    }

    /// <summary>
    /// 获取音效
    /// </summary>
    public AudioClip GetSoundEffect(ResourcePathAttribute resourcePath){
        return Get<AudioClip>("soundEffect", resourcePath);
    }

    /// <summary>
    /// 获取对话音效
    /// </summary>
    public AudioClip GetSoundDialogue(ResourcePath resourcePath){
        return Get<AudioClip>("soundDialogue", resourcePath);
    }
    
    /// <summary>
    /// 获取对话音效
    /// </summary>
    public AudioClip GetSoundDialogue(ResourcePathAttribute resourcePath){
        return Get<AudioClip>("soundDialogue", resourcePath);
    }
#endregion
    #region 文本文件
    /// <summary>
    /// 获取CSV文件
    /// </summary>
    public TextAsset GetCsvFile(ResourcePathAttribute resourcePath)
    {
        return Get<TextAsset>("csv", resourcePath);
    }

    /// <summary>
    /// 获取CSV文件
    /// </summary>
    public TextAsset GetCsvFile(ResourcePath resourcePath){
        return Get<TextAsset>("csv", resourcePath);
    }
#endregion
#region 图像文件
    /// <summary>
    /// 获取Sprite文件
    /// </summary>
    public Sprite GetSprite(ResourcePathAttribute resourcePath){
        return Get<Sprite>("sprite", resourcePath);
    }
    /// <summary>
    /// 获取Sprite文件
    /// </summary>
    public Sprite GetSprite(ResourcePath resourcePath){
        return Get<Sprite>("sprite", resourcePath);
    }
#endregion
#region 预制体
    /// <summary>
    /// 获取预制体
    /// </summary>
    public GameObject GetPrefab(ResourcePathAttribute resourcePath){
        return Get<GameObject>("prefab", resourcePath);
    }

    /// <summary>
    /// 获取预制体
    /// </summary>
    public GameObject GetPrefab(ResourcePath resourcePath){

        return Get<GameObject>("prefab", resourcePath);
    }
#endregion
#region 自定义Assets信息
    public T GetAssets<T>(string abName, ResourcePathAttribute resourcePath) where T :  UnityEngine.Object
    {
        return Get<T>(abName, resourcePath.AB_FileNmae, resourcePath.AssetsPath);
    }
    
    public T GetAssets<T>(string abName, ResourcePath resourcePath) where T : UnityEngine.Object
    {
        return Get<T>(abName, resourcePath.AB_FileNmae, resourcePath.AssetsPath);
    }
#endregion

    private T Get<T>(string abName, ResourcePathAttribute resourcePath) where T :  UnityEngine.Object
    {
        return Get<T>(abName, resourcePath.AB_FileNmae, resourcePath.AssetsPath);
    }

    private T Get<T>(string abName, ResourcePath resourcePath) where T :  UnityEngine.Object
    {
        return Get<T>(abName, resourcePath.AB_FileNmae, resourcePath.AssetsPath);
    }

    private T Get<T>(string abName, string ab_FileNmae, string AssetsPath) where T :  UnityEngine.Object
    {
#if UNITY_EDITOR
        if(FrameworkManager.IsEditorUsingAssetBundle){
            return aBManager.LoadAsset<T>(abName, ab_FileNmae);
        }
        else{
            return AssetDatabase.LoadAssetAtPath<T>(AssetsPath);
        }
#else
        return aBManager.LoadAsset<T>(abName, ab_FileNmae);
#endif
    }
}
