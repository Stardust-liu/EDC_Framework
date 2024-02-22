using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class WindowPrefabInfo{
    public GameObject prefab;
    [LabelText("在关闭完成后销毁对象")]
    public bool isHideFinishDestroy;
}

[CreateAssetMenu(fileName = "WindowPrefabSetting", menuName = "创建Assets文件/WindowPrefabSetting")]
public class WindowPrefabSetting : SerializedScriptableObject
{
    [DictionaryDrawerSettings(KeyLabel = "UI名字", ValueLabel ="UI对象信息")]
    public Dictionary<string, WindowPrefabInfo> uiPrefab;

    /// <summary>
    /// 获取对应的UI预制体信息
    /// </summary>
    public WindowPrefabInfo GetPrefabInfo(string name){
        return uiPrefab[name];
    }
}
