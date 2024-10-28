using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ViewSetting", menuName = "创建.Assets文件/ViewSetting")]
public class ViewPrefabSetting : SerializedScriptableObject
{
    [DictionaryDrawerSettings(KeyLabel = "UI名字", ValueLabel ="UI对象信息")]
    public Dictionary<string, UIPrefabInfo> viewPrefab;

    /// <summary>
    /// 获取对应的UI预制体信息
    /// </summary>
    public UIPrefabInfo GetPrefabInfo(string name){
        return viewPrefab[name];
    }
}
