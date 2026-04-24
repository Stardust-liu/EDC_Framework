using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LabelManager
{
    private readonly List<string> labelNames;
    private LabelManager(string _labelName)
    {
        labelNames = new List<string>(){_labelName};
    }
    private LabelManager(List<string> _labelNames)
    {
        this.labelNames = _labelNames;
    }

    //方便赋值后可以直接调用加载方法
    public static LabelManager Init(out LabelManager instance, string _labelName)
    {
        instance = new LabelManager(_labelName);
        return instance;
    }

    public static LabelManager Init(out LabelManager instance,  List<string> _labelNames)
    {
        instance = new LabelManager(_labelNames);
        return instance;
    }

    /// <summary>
    /// 加载Label
    /// </summary>
    public async UniTask LoadLabel()
    {
        var count = labelNames.Count;
        var task = new UniTask[count];
        for (var i = 0; i < count; i++)
        {
            task[i] = ((IResourcesModule)Hub.Resources).LoadLabel(labelNames[i]);
        }
        await UniTask.WhenAll(task);
    }

    /// <summary>
    /// 添加并加载Label
    /// </summary>
    public async UniTask AddLoadLabel(string labelName)
    {
        if (!labelNames.Contains(labelName))
        {
            labelNames.Add(labelName);
            await ((IResourcesModule)Hub.Resources).LoadLabel(labelName);
        }
        else
        {
            LogManager.LogWarning($"已加载 {labelName} 资源");
        }
    }

    /// <summary>
    /// 释放Label
    /// </summary>
    public void ReleaseLabel(string labelName)
    {
        if (labelNames.Contains(labelName))
        {
            ((IResourcesModule)Hub.Resources).ReleaseLabel(labelName);
            labelNames.Remove(labelName);
        }
    }

    /// <summary>
    /// 释放Label
    /// </summary>
    public void ReleaseLabelAll()
    {
        foreach (var item in labelNames)
        {
            ((IResourcesModule)Hub.Resources).ReleaseLabel(item);
        }
    }
}
