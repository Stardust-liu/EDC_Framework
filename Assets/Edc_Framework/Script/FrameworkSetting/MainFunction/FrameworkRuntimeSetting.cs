using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "FrameworkRuntimeSetting", menuName = "创建.Assets文件/FrameworkTool/FrameworkRuntimeSetting")]
public class FrameworkRuntimeSetting : SerializedScriptableObject
{
    [LabelText("显示打印信息的种类")]
    public LogLevel logDisplay;
    
    [LabelText("存档槽数量"), MinValue(1)]
    public int archiveSlotCount = 3;

    [LabelText("是否禁用数据保存功能")]
    [Tooltip("禁用时仍可读取和修改内存数据，但不会保存、复制或删除存档，也不会刷新保存时间。已开始的异步保存不受本次切换影响。")]
    public bool isSaveDisabled;

    [LabelText("启动时是否显示LOGO")]
    public bool isShowLogo;
}
