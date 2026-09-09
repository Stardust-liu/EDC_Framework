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


internal sealed class LabelConfigManagerRule : IEdcCheckRule
{
    public string RuleName => "配置管理器Label检查";

    public IEnumerable<EdcCheckResult> Check(EdcCheckContext context)
    {
        if (context.AddressableSettings == null)
        {
            yield break;
        }

        var managerTypes = TypeCache.GetTypesDerivedFrom<BaseLabelConfigManager>();
        foreach (var type in managerTypes)
        {
            if (type.IsAbstract || type.IsGenericTypeDefinition)
            {
                continue;
            }

            var script = EdcCheckerUtility.GetScript(type);
            if (!TryGetLabelNames(type, out var labelNames, out var errorMessage))
            {
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Error,
                    EdcCheckCategory.FrameworkConfig,
                    RuleName,
                    $"无法读取 {type.Name}.LabelNames：{errorMessage}",
                    script);
                continue;
            }

            if (labelNames == null || labelNames.Count == 0)
            {
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Warning,
                    EdcCheckCategory.FrameworkConfig,
                    RuleName,
                    $"配置管理器 {type.Name} 没有声明任何 Label",
                    script);
                continue;
            }

            var repeatedLabels = new HashSet<string>(StringComparer.Ordinal);
            foreach (var label in labelNames)
            {
                if (string.IsNullOrWhiteSpace(label))
                {
                    yield return new EdcCheckResult(
                        EdcCheckSeverity.Error,
                        EdcCheckCategory.FrameworkConfig,
                        RuleName,
                        $"配置管理器 {type.Name} 的 LabelNames 中存在空字符串",
                        script);
                    continue;
                }

                if (!repeatedLabels.Add(label))
                {
                    yield return new EdcCheckResult(
                        EdcCheckSeverity.Warning,
                        EdcCheckCategory.FrameworkConfig,
                        RuleName,
                        $"配置管理器 {type.Name} 重复声明了 Label：{label}",
                        script);
                }

                if (!context.HasDefinedLabel(label))
                {
                    yield return new EdcCheckResult(
                        EdcCheckSeverity.Error,
                        EdcCheckCategory.FrameworkConfig,
                        RuleName,
                        $"配置管理器 {type.Name} 会加载 Label {label}，但 Addressables 中没有定义该 Label",
                        script);
                    continue;
                }

                if (!context.HasUsedLabel(label))
                {
                    yield return new EdcCheckResult(
                        EdcCheckSeverity.Error,
                        EdcCheckCategory.FrameworkConfig,
                        RuleName,
                        $"配置管理器 {type.Name} 会加载 Label {label}，但没有任何 Addressables 资源使用该 Label",
                        script);
                }
            }
        }
    }

    private static bool TryGetLabelNames(Type managerType, out List<string> labelNames, out string errorMessage)
    {
        labelNames = null;
        errorMessage = null;

        var property = managerType.GetProperty("LabelNames", BindingFlags.Instance | BindingFlags.NonPublic);
        if (property == null)
        {
            errorMessage = "没有找到 LabelNames 属性";
            return false;
        }

        try
        {
            var manager = Activator.CreateInstance(managerType);
            labelNames = property.GetValue(manager) as List<string>;
            return true;
        }
        catch (Exception exception)
        {
            errorMessage = exception.Message;
            return false;
        }
    }
}

