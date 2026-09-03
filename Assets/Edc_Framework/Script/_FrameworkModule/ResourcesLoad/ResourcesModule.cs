using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

public interface IResourceOwnerFactory
{
    /// <summary>
    /// 创建资源持有者
    /// </summary>
    IResourceOwner CreateOwner(string ownerName = null);
}

internal interface IResourcesModule
{
    /// <summary>
    /// 加载Label
    /// </summary>
    UniTask LoadLabel(string labelName);
    /// <summary>
    /// 释放Label
    /// </summary>
    void ReleaseLabel(string labelName);
    
    /// <summary>
    /// 加载资源
    /// </summary>
    UniTask Load(string key);

    /// <summary>
    /// 加载资源
    /// </summary>
    void WaitLoadCompletion(string key);

    /// <summary>
    /// 释放资源
    /// </summary>
    void Release(string labelName);

    /// <summary>
    /// 获取资源
    /// </summary>
    T Get<T>(string labelName, string key) where T : Object;

    /// <summary>
    /// 获取资源
    /// </summary>
    T Get<T>(string key) where T : Object;

    /// <summary>
    /// 释放资源持有者
    /// </summary>
    void UnregisterOwner(ResourceOwner resourceOwner);
} 

public class ResourceRef
{
    public int refCount;
    public AsyncOperationHandle handle;
    public ResourceRef(AsyncOperationHandle _handle)
    {
        handle = _handle;
        refCount = 1; // 初始通常为 1
    }
}

public class ListResourceRef
{
    public int refCount;
    public List<AsyncOperationHandle> handleList;
    public ListResourceRef(List<AsyncOperationHandle> _handle)
    {
        handleList = _handle;
        refCount = 1; // 初始通常为 1
    }
}

internal class ResourcesModule : BaseIOCComponent, IResourcesModule, IResourceOwnerFactory
{
    private static readonly Dictionary<string, ResourceRef> loadedHandles = new();
    private static readonly Dictionary<string, ListResourceRef> loadedHandlesLabel = new();
    private static readonly Dictionary<string, Dictionary<string,Object>> loadedLabelAssets = new();
    private static readonly Dictionary<string, Object> loadedAssets = new();
    private readonly Dictionary<string, UniTask> loadingLabelTasks = new();
    private readonly HashSet<ResourceOwner> activeOwners = new();

    protected override void Uninstall()
    {
        base.Uninstall();
        ReleaseAllOwners();
    }

    /// <summary>
    /// 获取资源
    /// </summary>
    public T Get<T>(string key) where T : Object
    {
        if (!string.IsNullOrEmpty(key))
        {
            if (loadedAssets.TryGetValue(key, out var value))
            {
                return value as T;
            }
            foreach (var item in loadedLabelAssets)
            {
                if (item.Value.TryGetValue(key, out value))
                {
                    return value as T;
                }
            }
            Debug.LogError($"找不到{key}资源");
            return null;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 获取Label资源
    /// </summary>
    public T Get<T>(string labelName, string key) where T : Object
    {
        if (!string.IsNullOrEmpty(labelName) && !string.IsNullOrEmpty(key))
        {
            if (loadedLabelAssets.TryGetValue(labelName, out var labelInfo)&&
                labelInfo.TryGetValue(key, out var value))
            {
                return value as T;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }

#region Label资源
    /// <summary>
    /// 加载Label
    /// </summary>
    async UniTask IResourcesModule.LoadLabel(string labelName)
    {
        if (loadingLabelTasks.TryGetValue(labelName, out var ongoingTask))
        {
            loadedHandlesLabel[labelName].refCount++;
            await ongoingTask; 
            return;
        }
        if (loadedHandlesLabel.ContainsKey(labelName))
        {
            loadedHandlesLabel[labelName].refCount++;
            return;
        }
        var task = LoadLabelTask(labelName).Preserve();
        loadingLabelTasks[labelName] = task;
        try 
        {
            await task;
        } 
        finally 
        {
            loadingLabelTasks.Remove(labelName); // 结束后移除任务记录
        }
    }

    async UniTask LoadLabelTask(string labelName)
    {
        loadedLabelAssets.Add(labelName, new Dictionary<string, Object>());
        loadedHandlesLabel.Add(labelName, new ListResourceRef(new List<AsyncOperationHandle>()));
        var locationsHandle = Addressables.LoadResourceLocationsAsync(labelName);
        try
        {
            await locationsHandle.ToUniTask();
            var count = locationsHandle.Result.Count;
            var keys = new List<string>();
            var tempTasks = new List<UniTask<Object>>(count);
            for (var i = 0; i < count; i++)
            {
                var key = locationsHandle.Result[i].PrimaryKey;
                if (keys.Contains(key)) continue;
                var handle = Addressables.LoadAssetAsync<Object>(key);
                keys.Add(key);
                loadedHandlesLabel[labelName].handleList.Add(handle);
                tempTasks.Add(handle.ToUniTask());
            }
            var results = await UniTask.WhenAll(tempTasks);
            count = keys.Count;
            for (int i = 0; i < count; i++)
            {
                loadedLabelAssets[labelName][keys[i]] = results[i];
            }
        }
        catch (System.Exception)
        {
            foreach (var item in loadedHandlesLabel[labelName].handleList)
            {
                if (item.IsValid())
                {
                    Addressables.Release(item);
                }
            }
            loadedHandlesLabel.Remove(labelName);
            loadedLabelAssets.Remove(labelName);            
            throw;
        }
        finally
        {
            Addressables.Release(locationsHandle);
        }
    }

    /// <summary>
    /// 释放Label
    /// </summary>
    void IResourcesModule.ReleaseLabel(string labelName)
    {
        if (loadedHandlesLabel.TryGetValue(labelName, out var listResourceRef))
        {
            listResourceRef.refCount --;
            if (listResourceRef.refCount == 0)
            {
                foreach (var item in listResourceRef.handleList)
                {
                    if (item.IsValid())
                    {
                        Addressables.Release(item);
                    }
                }
                loadedLabelAssets.Remove(labelName);
                loadedHandlesLabel.Remove(labelName);   
            }
        }
    }
#endregion
#region 单个资源
    /// <summary>
    /// 加载资源
    /// </summary>
    async UniTask IResourcesModule.Load(string key)
    {
        bool isFirstCaller = false;
        if (!loadedHandles.TryGetValue(key, out ResourceRef resourceRef))
        {
            resourceRef = new ResourceRef(Addressables.LoadAssetAsync<Object>(key));
            loadedHandles.Add(key, resourceRef);
            isFirstCaller = true;
        }
        else
        {
            resourceRef.refCount++;
        }

        try
        {
            var res = await resourceRef.handle.Convert<Object>().ToUniTask();
            loadedAssets[key] = res;
        }
        catch (System.Exception)
        {
            if (isFirstCaller)
            {
                if (resourceRef.handle.IsValid())
                {
                    Addressables.Release(resourceRef.handle);
                }
                loadedHandles.Remove(key);
            }
            throw; 
        }
    }

    /// <summary>
    /// 同步加载资源
    /// </summary>
    void IResourcesModule.WaitLoadCompletion(string key)
    {
        if (!loadedHandles.TryGetValue(key, out ResourceRef resourceRef))
        {
            resourceRef = new ResourceRef(Addressables.LoadAssetAsync<Object>(key));
            resourceRef.handle.WaitForCompletion(); 
            loadedHandles.Add(key, resourceRef);
            loadedAssets[key] = resourceRef.handle.Result as Object;
        }
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    void IResourcesModule.Release(string key)
    {
        if (loadedHandles.TryGetValue(key, out var resourceRef))
        {
            resourceRef.refCount--;
            if (resourceRef.refCount == 0)
            {
                loadedAssets.Remove(key);
                loadedHandles.Remove(key);
                Addressables.Release(resourceRef.handle);   
            }
        }
    }
#endregion
    /// <summary>
    /// 创建资源持有者
    /// </summary>
    public IResourceOwner CreateOwner(string ownerName = null)
    {
        var owner = new ResourceOwner(this, ownerName);
        activeOwners.Add(owner);
        return owner;
    }

    /// <summary>
    /// 注销资源持有者
    /// </summary>
    public void UnregisterOwner(ResourceOwner resourceOwner)
    {
        activeOwners.Remove(resourceOwner);
    }

    /// <summary>
    /// 释放所有资源持有者
    /// </summary>
    private void ReleaseAllOwners()
    {
        var owners = new List<ResourceOwner>(activeOwners);
        foreach (var item in owners)
        {
            item.ReleaseAll();
        }
        activeOwners.Clear();
    }
}
