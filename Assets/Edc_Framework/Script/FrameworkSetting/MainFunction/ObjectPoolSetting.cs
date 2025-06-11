using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
[CreateAssetMenu(fileName = "ObjectPoolSetting", menuName = "创建.Assets文件/FrameworkTool/ObjectPoolSetting")]
public class ObjectPoolSetting : SerializedScriptableObject
{
    [ReadOnly]
    [DictionaryDrawerSettings(KeyLabel = "预制体名字", ValueLabel ="预制体对象")]
    public Dictionary<string, GameObject> frameworkPrefab;

    [DictionaryDrawerSettings(KeyLabel = "预制体名字", ValueLabel ="预制体对象")]
    public Dictionary<string, GameObject> universalPrefab;

    public GameObject GetPool(string poolName, string sceneName = null){
        if(frameworkPrefab.TryGetValue(poolName, out var prefab)){
            return prefab;
        }
        if(universalPrefab.TryGetValue(poolName, out prefab)){
            return prefab;
        }
        switch (sceneName)
        {
            default:
                LogManager.LogError($"sceneName填写错误，没有针对 {sceneName} 的处理逻辑");
                return null;
        }
    }
}
