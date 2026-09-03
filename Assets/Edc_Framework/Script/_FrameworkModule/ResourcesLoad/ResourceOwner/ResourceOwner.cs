using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ResourceOwner : IResourceOwner
{
    private readonly IResourcesModule resources;
    private bool isReleased;
    public string OwnerName { get; }
    private LabelResourceSet labelResourceSet;
    private LabelResourceSet LabelResourceSet
    {
        get
        {
            labelResourceSet ??= new LabelResourceSet(resources, OwnerName);
            return labelResourceSet;
        }
    }

    private AssetResourceSet assetResourceSet;
    private AssetResourceSet AssetResourceSet
    {
        get
        {
            assetResourceSet ??= new AssetResourceSet(resources, OwnerName);
            return assetResourceSet;
        }
    }

    internal ResourceOwner(IResourcesModule _resources, string _ownerName)
    {
        resources = _resources;
        OwnerName = string.IsNullOrEmpty(_ownerName) ? "UnnamedResourceOwner" : _ownerName;
    }

    /// <summary>
    /// 加载Label
    /// </summary>
    public UniTask LoadLabel(string labelName)
    {
        if (!CheckAvailable())
        {
            return UniTask.CompletedTask;
        }
        return LabelResourceSet.LoadLabel(labelName);
    }

    /// <summary>
    /// 释放指定Label
    /// </summary>
    public void ReleaseLabel(string labelName)
    {
        if (!CheckAvailable())
        {
            return;
        }
        labelResourceSet?.ReleaseLabel(labelName);
    }

    /// <summary>
    /// 加载资源
    /// </summary>
    public UniTask LoadAsset(string keyName)
    {
        if (!CheckAvailable())
        {
            return UniTask.CompletedTask;
        }
        return AssetResourceSet.LoadAsset(keyName);
    }

    /// <summary>
    /// 释放指定资源
    /// </summary>
    public void ReleaseAsset(string keyName)
    {
        if (!CheckAvailable())
        {
            return;
        }
        assetResourceSet?.ReleaseAsset(keyName);
    }

    /// <summary>
    /// 释放所有资源
    /// </summary>
    public void ReleaseAll()
    {
        if (isReleased)
        {
            return;
        }
        labelResourceSet?.ReleaseAll();
        labelResourceSet = null;
        assetResourceSet?.ReleaseAll();
        assetResourceSet = null;
        isReleased = true;
        resources.UnregisterOwner(this);
    }

    /// <summary>
    /// 获取Label资源
    /// </summary>
    public T GetLabelAsset<T>(string labelName, string keyName) where T : Object
    {
        if (!CheckAvailable())
        {
            return null;
        }
        if (labelResourceSet == null)
        {
            LogManager.LogWarning($"资源持有者 {OwnerName} 未加载任何 Label，无法获取 {keyName}");
            return null;
        }
        return labelResourceSet.GetLabelAsset<T>(labelName, keyName);
    }

    /// <summary>
    /// 获取Asset资源
    /// </summary>
    public T GetAsset<T>(string keyName) where T : Object
    {
        if (!CheckAvailable())
        {
            return null;
        }
        if (assetResourceSet == null)
        {
            LogManager.LogWarning($"资源持有者 {OwnerName} 未加载任何 Asset，无法获取 {keyName}");
            return null;
        }
        return assetResourceSet.GetAsset<T>(keyName);
    }

    /// <summary>
    /// 尝试获取Asset资源，资源不存在时不打印警告
    /// </summary>
    internal T TryGetAssetAndLabelAsset<T>(string keyName) where T : Object
    {
        if (!CheckAvailable())
        {
            return null;
        }
        var result = assetResourceSet?.GetAsset<T>(keyName, isLogWarning: false);
        if(result != null)
        {
            return result;
        }
        result = labelResourceSet?.GetAsset<T>(keyName);
        return result;
    }

    private bool CheckAvailable()
    {
        if (!isReleased)
        {
            return true;
        }
        LogManager.LogWarning($"资源持有者 {OwnerName} 已释放，不能继续使用");
        return false;
    }
}

public interface IResourceOwner : IResourceSet, ILabelResourceSet, IAssetResourceSet
{
    string OwnerName { get; }
}

public interface IResourceSet
{
    void ReleaseAll();
}

public interface ILabelResourceSet
{
    UniTask LoadLabel(string labelName);
    void ReleaseLabel(string labelName);
    T GetLabelAsset<T>(string labelName, string keyName) where T : Object;
}
public interface IAssetResourceSet
{
    UniTask LoadAsset(string keyName);
    void ReleaseAsset(string keyName);
    T GetAsset<T>(string keyName) where T : Object;
}

internal sealed class LabelResourceSet : ILabelResourceSet
{
    private readonly HashSet<string> labelKeys = new();
    private readonly Dictionary<string, UniTask> loadingTasks = new();
    private readonly IResourcesModule resources;
    private readonly string ownerName;

    internal LabelResourceSet(IResourcesModule _resources, string _ownerName)
    {
        resources = _resources;
        ownerName = _ownerName;
    } 

    public async UniTask LoadLabel(string labelName)
    {
        if (labelKeys.Contains(labelName))
        {
            if (loadingTasks.TryGetValue(labelName, out var loadingTask))
            {
                await loadingTask;
                return;
            }

            LogManager.LogWarning($"已加载 {labelName} 资源");
            return;
        }

        labelKeys.Add(labelName);
        var task = resources.LoadLabel(labelName).Preserve();
        loadingTasks[labelName] = task;
        try
        {
            await task;
        }
        catch
        {
            labelKeys.Remove(labelName);
            LogManager.LogWarning($"{labelName} 加载失败");
            throw;
        }
        finally
        {
            loadingTasks.Remove(labelName);
        }
    }

    public void ReleaseLabel(string labelName)
    {
        if (labelKeys.Remove(labelName))
        {
            resources.ReleaseLabel(labelName);
        }
    }

    public void ReleaseAll()
    {
        foreach (var item in labelKeys)
        {
            resources.ReleaseLabel(item);
        }
        labelKeys.Clear();
        loadingTasks.Clear();
    }

    public T GetLabelAsset<T>(string labelName, string keyName) where T : Object
    {
        if (!labelKeys.Contains(labelName))
        {
            LogManager.LogWarning($"资源持有者 {ownerName} 未加载 Label {labelName}，无法获取 {keyName}");
            return null;
        }
        return resources.Get<T>(labelName, keyName);
    }

    internal T GetAsset<T>(string keyName) where T : Object
    {
        foreach (var labelName in labelKeys)
        {
            var asset = resources.Get<T>(labelName, keyName);
            if (asset != null)
            {
                return asset;
            }
        }

        return null;
    }
}

internal sealed class AssetResourceSet : IAssetResourceSet
{
    private readonly HashSet<string> keyNames = new();
    private readonly Dictionary<string, UniTask> loadingTasks = new();
    private readonly IResourcesModule resources;
    private readonly string ownerName;

    internal AssetResourceSet(IResourcesModule _resources, string _ownerName)
    {
        resources = _resources;
        ownerName = _ownerName;
    }

    /// <summary>
    /// 添加并加载资源
    /// </summary>
    public async UniTask LoadAsset(string keyName)
    {
        if (keyNames.Contains(keyName))
        {
            if (loadingTasks.TryGetValue(keyName, out var loadingTask))
            {
                await loadingTask;
                return;
            }

            LogManager.LogWarning($"已加载 {keyName} 资源");
            return;
        }

        keyNames.Add(keyName);
        var task = resources.Load(keyName).Preserve();
        loadingTasks[keyName] = task;
        try
        {
            await task;
        }
        catch
        {
            keyNames.Remove(keyName);
            LogManager.LogWarning($"{keyName} 加载失败");
            throw;
        }
        finally
        {
            loadingTasks.Remove(keyName);
        }
    }

    /// <summary>
    /// 释放指定资源
    /// </summary>
    public void ReleaseAsset(string keyName)
    {
        if (keyNames.Remove(keyName))
        {
            resources.Release(keyName);
        }
    }

    /// <summary>
    /// 释放所有资源
    /// </summary>
    public void ReleaseAll()
    {
        foreach (var item in keyNames)
        {
            resources.Release(item);
        }
        keyNames.Clear();
        loadingTasks.Clear();
    }

    public T GetAsset<T>(string keyName) where T : Object
    {
        return GetAsset<T>(keyName, isLogWarning: true);
    }

    internal T GetAsset<T>(string keyName, bool isLogWarning) where T : Object
    {
        if (!keyNames.Contains(keyName))
        {
            if (isLogWarning)
            {
                LogManager.LogWarning($"资源持有者 {ownerName} 未加载资源 {keyName}，无法获取");
            }
            return null;
        }
        return resources.Get<T>(keyName);
    }
}