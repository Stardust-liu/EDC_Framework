using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "PersistentViewSetting", menuName = "创建.Assets文件/PersistentViewSetting")]
public class PersistentViewPrefabSetting : SerializedScriptableObject
{
    [DictionaryDrawerSettings(KeyLabel = "UI名字", ValueLabel ="UI对象信息")]
    public Dictionary<string, UIPrefabInfo> persistentUiPrefab;
    
    /// <summary>
    /// 获取对应的常驻UI信息
    /// </summary>
    public UIPrefabInfo GetPersistentPrefabInfo(string name){
        return persistentUiPrefab[name];
    }
}
