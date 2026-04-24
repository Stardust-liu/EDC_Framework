using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
[Serializable]
public class UIPrefabInfo
{
    [LabelText("预制体_2DUI")]
    public string prefab_2D; 
    [LabelText("预制体_3DUI")]
    public string prefab_3D; 
    [LabelText("在关闭完成后销毁对象")]
    public bool isHideFinishDestroy;
}

[Serializable]
public class UIEntry
{
    public string name;
    public UIPrefabInfo info;
}
public class UIPrefabSetting : SerializedScriptableObject
{
    public List<UIEntry> panelList;
    private Dictionary<string, UIPrefabInfo> panelInfoDict;

    public void Init()
    {
        if(panelInfoDict == null)
        {
            panelInfoDict = new Dictionary<string, UIPrefabInfo>();
        }
        else
        {
            panelInfoDict?.Clear();
        }
        foreach (var item in panelList)
        {
            panelInfoDict.Add(item.name, item.info);
        }
#if !UNITY_EDITOR
        panelList = null;
#endif
    }

    /// <summary>
    /// 获取对应的UI预制体信息
    /// </summary>
    public UIPrefabInfo GetPanelInfo(string panelName){
        if(panelInfoDict.TryGetValue(panelName, out UIPrefabInfo uIPrefabInfo)){
            return uIPrefabInfo;
        }
        else{
            return null;
        }
    }
}
