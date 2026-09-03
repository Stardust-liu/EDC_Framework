using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BaseLabelConfigManager : BaseIOCComponent
{
    protected virtual List<string> LabelNames => null;
    private IResourceOwner resourceOwner;

    protected override void Init()
    {
        base.Init();
        resourceOwner = Hub.Resources.CreateOwner(GetType().Name);
    }

    /// <summary>
    /// 加载Label
    /// </summary>
    public async UniTask LoadLabel()
    {
        var labelNames = LabelNames;
        if (labelNames == null || labelNames.Count == 0)
        {
            return;
        }

        var tasks = new UniTask[labelNames.Count];
        for (var i = 0; i < labelNames.Count; i++)
        {
            tasks[i] = resourceOwner.LoadLabel(labelNames[i]);
        }

        await UniTask.WhenAll(tasks);
    }

    /// <summary>
    /// 释放Label
    /// </summary>
    public void ReleaseLabel()
    {
        resourceOwner?.ReleaseAll();
        resourceOwner = null;
    }

    /// <summary>
    /// 获取配置资源
    /// </summary>
    public T Get<T>(string keyName) where T : Object
    {
        if (resourceOwner == null)
        {
            LogManager.LogWarning($"{GetType().Name} 未初始化资源持有者，无法获取 {keyName}");
            return null;
        }
        foreach (var labelName in LabelNames)
        {
            var asset = resourceOwner.GetLabelAsset<T>(labelName, keyName);
            if (asset != null)
            {
                return asset;
            }
        }
        LogManager.LogWarning($"{GetType().Name} 未找到配置资源 {keyName}，请检查资源是否已添加到对应 Label");        
        return null;
    }
}
