using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
public class UIPrefabInfo
{
    [LabelText("预制体")]
    public GameObject prefab;
    [LabelText("在关闭完成后销毁对象")]
    public bool isHideFinishDestroy;
    [LabelText("是否是3DUI")]
    public bool is3DUI;
}

public class UIPrefabSetting : SerializedScriptableObject
{
    [DictionaryDrawerSettings(KeyLabel = "UI名字", ValueLabel ="UI对象信息")]
    public Dictionary<string, UIPrefabInfo> panelInfo;
    
    /// <summary>
    /// 获取对应的UI预制体信息
    /// </summary>
    public UIPrefabInfo GetPanelInfo(string panelName){
        if(panelInfo.TryGetValue(panelName, out UIPrefabInfo uIPrefabInfo)){
            return uIPrefabInfo;
        }
        else{
            return null;
        }
    }
}
