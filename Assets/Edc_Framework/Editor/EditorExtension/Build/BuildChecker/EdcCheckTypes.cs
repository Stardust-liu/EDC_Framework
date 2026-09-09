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


public enum EdcCheckSeverity
{
    Info,
    Warning,
    Error,
}

public enum EdcCheckCategory
{
    Addressables,
    FrameworkConfig,
    UI,
    Scene,
    ObjectPool,
    Build,
    Archive,
}

[Serializable]
public class EdcCheckResult
{
    [HideLabel, ReadOnly, TableColumnWidth(140, true), LabelText("等级")]
    public EdcCheckSeverity Severity;

    [HideLabel, ReadOnly, TableColumnWidth(140, true), LabelText("分类")]
    public EdcCheckCategory Category;

    [HideLabel, TableColumnWidth(140, true), LabelText("规则")]
    public string RuleName;

    [HideLabel, TableColumnWidth(320, true), LabelText("说明")]
    public string Message;

    [HideLabel, TableColumnWidth(420, true), LabelText("资源路径")]
    public string AssetPath;

    [NonSerialized]
    private Object context;

    public EdcCheckResult(
        EdcCheckSeverity severity,
        EdcCheckCategory category,
        string ruleName,
        string message,
        Object context = null,
        string assetPath = null)
    {
        Severity = severity;
        Category = category;
        RuleName = ruleName;
        Message = message;
        this.context = context;
        AssetPath = assetPath ?? (context == null ? string.Empty : AssetDatabase.GetAssetPath(context));
    }

    [Button("定位")]
    [ShowIf(nameof(CanLocate))]
    [TableColumnWidth(100, true)]
    private void Locate()
    {
        Selection.activeObject = context;
        EditorGUIUtility.PingObject(context);
    }

    private bool CanLocate()
    {
        return context != null;
    }
}

