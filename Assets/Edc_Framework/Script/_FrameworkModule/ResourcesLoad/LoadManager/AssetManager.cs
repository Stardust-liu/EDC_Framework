using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class AssetManager
{
    private List<string> keyNames;
    public List<string> KeyNames{get{return keyNames;}}

    private AssetManager(string _keyName)
    {
        keyNames = new List<string>(){_keyName};
    }

    private AssetManager(List<string> _keyNames)
    {
        this.keyNames = _keyNames;
    }

    //方便赋值后可以直接调用加载方法
    public static AssetManager Init(out AssetManager instance, string _keyName)
    {
        instance = new AssetManager(_keyName);
        return instance;
    }

    public static AssetManager Init(out AssetManager instance, List<string> _keyNames)
    {
        instance = new AssetManager(_keyNames);
        return instance;
    }

    /// <summary>
    /// 加载资源
    /// </summary>
    public async UniTask Load()
    {
        var count = keyNames.Count;
        var task = new UniTask[count];
        for (var i = 0; i < count; i++)
        {
            task[i] = ((IResourcesModule)Hub.Resources).Load(keyNames[i]);
        }
        await UniTask.WhenAll(task);
    }

    /// <summary>
    /// 添加并加载资源
    /// </summary>
    public async UniTask AddLoad(string keyName)
    {
        if (!keyNames.Contains(keyName))
        {
            keyNames.Add(keyName);
            await ((IResourcesModule)Hub.Resources).Load(keyName);
        }
        else
        {
            LogManager.LogWarning($"已加载 {keyNames} 资源");
        }
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    public void Release(string keyName)
    {
        if (keyNames.Contains(keyName))
        {
            ((IResourcesModule)Hub.Resources).Release(keyName);
            keyNames.Remove(keyName);
        }
    }

    /// <summary>
    /// 释放所有资源
    /// </summary>
    public void ReleaseAll()
    {
        foreach (var item in keyNames)
        {
            ((IResourcesModule)Hub.Resources).Release(item);
        }
        keyNames.Clear();
    }
}
