using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "FrameworkRuntimeSetting", menuName = "创建.Assets文件/FrameworkTool/FrameworkRuntimeSetting")]
public class FrameworkRuntimeSetting : SerializedScriptableObject
{
    [LabelText("显示打印信息的种类")]
    public LogLevel logDisplay;

    [LabelText("是否禁用数据保存功能")]
    public bool isSaveDisabled;

    [LabelText("Editor下使用Ab包加载数据")]
    public bool isEditorUsingAssetBundle;
}
