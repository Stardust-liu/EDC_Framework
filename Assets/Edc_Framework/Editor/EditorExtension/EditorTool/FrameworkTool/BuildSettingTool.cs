using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildSettingTool", menuName = "创建.Assets文件/FrameworkTool/BuildSettingTool")]
public class BuildSettingTool : SerializedScriptableObject
{
    public enum BuildChannels{
        Steam,
    }

    [LabelText("构建渠道")]
    [LabelWidth(55)]
    public BuildChannels buildChannels;
}

