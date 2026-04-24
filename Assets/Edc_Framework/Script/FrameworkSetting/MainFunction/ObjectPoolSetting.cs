using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

[Serializable]
public class PoolEntry
{
    public string name;
    public PoolInfo info;
}

[Serializable]
public class PoolInfo
{
    public string keyName;
}

[CreateAssetMenu(fileName = "ObjectPoolSetting", menuName = "创建.Assets文件/FrameworkTool/ObjectPoolSetting")]
public class ObjectPoolSetting : SerializedScriptableObject
{
    public List<PoolEntry> prefabList;
    private Dictionary<string, PoolInfo> prefabInfoDict;

    public void Init()
    {
        if(prefabInfoDict == null)
        {
            prefabInfoDict = new Dictionary<string, PoolInfo>();
        }
        else
        {
            prefabInfoDict?.Clear();
        }
        foreach (var item in prefabList)
        {
            prefabInfoDict.Add(item.name, item.info);
        }
#if !UNITY_EDITOR
        prefabList = null;
#endif
    }

    public PoolInfo GetPool(string poolName){
        if(prefabInfoDict.TryGetValue(poolName, out var prefab)){
            return prefab;
        }
        else
        {
            Debug.LogError($"没有{poolName}对象池信息");
            return null;
        }
    }
}
