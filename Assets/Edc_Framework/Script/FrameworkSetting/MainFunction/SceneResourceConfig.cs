using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "SceneResourceConfig", menuName = "创建.Assets文件/FrameworkTool/SceneResourceConfig")]
public class SceneResourceConfig : SerializedScriptableObject
{
    [LabelText("切换场景自动销毁")]
    public bool isChangeSceneAutoRelease;
    public string[] addressableLabels;// 需要加载或释放的资源标签
    public string[] addressables;// 需要加载或释放的资源
    [System.NonSerialized]
    private bool isLoad;
    [System.NonSerialized]
    private IResourceOwner resourceOwner;

    /// <summary>
    /// 加载
    /// </summary>
    public async UniTask Load()
    {
        if(isLoad) return;
        resourceOwner = Hub.Resources.CreateOwner(name);
        var addressableLabelsCount = addressableLabels.Length;
        var addressablesCount = addressables.Length;
        var task = new UniTask[addressableLabelsCount + addressablesCount];
        for (var i = 0; i < addressableLabelsCount; i++)
        {
            task[i] = resourceOwner.LoadLabel(addressableLabels[i]);
        }
        for (var i = 0; i < addressablesCount; i++)
        {
            task[i+addressableLabelsCount] = resourceOwner.LoadAsset(addressables[i]);
        }
        await UniTask.WhenAll(task);
        isLoad = true;
    } 

    /// <summary>
    /// 释放
    /// </summary>
    public void Release()
    {
        isLoad = false;
        resourceOwner?.ReleaseAll();
        resourceOwner = null;
    }
}
