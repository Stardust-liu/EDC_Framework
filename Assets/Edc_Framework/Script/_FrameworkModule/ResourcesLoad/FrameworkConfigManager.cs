using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class FrameworkConfigManager : BaseLabelConfigManager
{
    protected override List<string> LabelNames => new List<string>
    {
        "FrameworkConfig"//框架配置文件
    };
}
