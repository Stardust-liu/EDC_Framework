using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


[CreateAssetMenu(fileName = "SceneResourcesSetting", menuName = "创建.Assets文件/FrameworkTool/SceneResourcesSetting")]
public class SceneResourcesSetting : SerializedScriptableObject
{
    public Dictionary<string, SceneResourceConfig> keyValuePairs;

    /// <summary>
    /// 获取关卡加载信息
    /// </summary>
    public SceneResourceConfig GetResourceConfig(string scenen)
    {
        return keyValuePairs.TryGetValue(scenen, out SceneResourceConfig sceneResource)? sceneResource : null;
    }
}
