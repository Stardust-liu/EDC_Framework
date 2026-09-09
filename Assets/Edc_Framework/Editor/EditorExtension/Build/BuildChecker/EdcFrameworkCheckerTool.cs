using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using Object = UnityEngine.Object;


[Serializable]
public class EdcFrameworkCheckerTool
{
    [ShowInInspector, ReadOnly, LabelText("错误")]
    private int ErrorCount => results.Count(item => item.Severity == EdcCheckSeverity.Error);

    [ShowInInspector, ReadOnly, LabelText("警告")]
    private int WarningCount => results.Count(item => item.Severity == EdcCheckSeverity.Warning);

    [ShowInInspector, ReadOnly, LabelText("信息")]
    private int InfoCount => results.Count(item => item.Severity == EdcCheckSeverity.Info);

    [ShowInInspector, ReadOnly, LabelText("最后扫描时间")]
    private string LastCheckTime => lastCheckTime.HasValue ? lastCheckTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "未扫描";

    [ShowInInspector]
    [TableList(AlwaysExpanded = true, HideToolbar = true, DefaultMinColumnWidth = 80)]
    [LabelText("检查结果")]
    private List<EdcCheckResult> results = new();

    private DateTime? lastCheckTime;

    [ButtonGroup("操作")]
    [Button("扫描全部", ButtonSizes.Large)]
    [GUIColor(0.5f, 0.8f, 1f)]
    private void CheckAll()
    {
        results = EdcFrameworkChecker.CheckAll();
        lastCheckTime = DateTime.Now;

        EdcFrameworkChecker.LogSummary(results);
    }

    [ButtonGroup("操作")]
    [Button("清空结果")]
    private void ClearResults()
    {
        results.Clear();
        lastCheckTime = null;
    }
}

