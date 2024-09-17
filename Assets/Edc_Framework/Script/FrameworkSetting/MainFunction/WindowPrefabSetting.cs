using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


[CreateAssetMenu(fileName = "WindowSetting", menuName = "创建.Assets文件/WindowSetting")]
public class WindowPrefabSetting : SerializedScriptableObject
{
    [DictionaryDrawerSettings(KeyLabel = "UI名字", ValueLabel ="UI对象信息")]
    public Dictionary<string, UIPrefabInfo> windowPrefab;

    /// <summary>
    /// 获取对应的UI预制体信息
    /// </summary>
    public UIPrefabInfo GetPrefabInfo(string name){
        return windowPrefab[name];
    }
}
