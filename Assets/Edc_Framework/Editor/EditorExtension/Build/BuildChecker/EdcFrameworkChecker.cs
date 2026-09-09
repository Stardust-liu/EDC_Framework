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


public static class EdcFrameworkChecker
{
    public static List<EdcCheckResult> CheckAll()
    {
        return EdcCheckRunner.CheckAll();
    }

    public static bool HasError(IReadOnlyList<EdcCheckResult> results)
    {
        return results != null && results.Any(item => item.Severity == EdcCheckSeverity.Error);
    }

    public static string BuildSummary(IReadOnlyList<EdcCheckResult> results)
    {
        if (results == null || results.Count == 0)
        {
            return "EDC框架检查完成：未发现问题";
        }

        var errorCount = results.Count(item => item.Severity == EdcCheckSeverity.Error);
        var warningCount = results.Count(item => item.Severity == EdcCheckSeverity.Warning);

        if (errorCount <= 0 && warningCount <= 0)
        {
            return "EDC框架检查完成：未发现错误或警告";
        }

        return $"EDC框架检查完成：{errorCount} 个错误，{warningCount} 个警告";
    }

    public static void LogSummary(IReadOnlyList<EdcCheckResult> results)
    {
        var summary = BuildSummary(results);
        if (HasError(results))
        {
            Debug.LogError(summary);
            return;
        }

        if (results != null && results.Any(item => item.Severity == EdcCheckSeverity.Warning))
        {
            Debug.LogWarning(summary);
            return;
        }

        Debug.Log(summary);
    }
}

