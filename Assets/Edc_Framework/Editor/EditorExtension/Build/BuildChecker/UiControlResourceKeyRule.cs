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


internal sealed class UiControlResourceKeyRule : IEdcCheckRule
{
    public string RuleName => "UI控制器ResourceKey检查";

    public IEnumerable<EdcCheckResult> Check(EdcCheckContext context)
    {
        var panelKeys = UiSettingRule.GetAllPanelKeys();
        var controlTypes = TypeCache.GetTypesDerivedFrom<BaseUIControl>();
        var usedKeys = new HashSet<string>(StringComparer.Ordinal);

        foreach (var type in controlTypes)
        {
            if (type.IsAbstract || type.IsGenericTypeDefinition)
            {
                continue;
            }

            var attribute = (ResourceKeyAttribute)Attribute.GetCustomAttribute(type, typeof(ResourceKeyAttribute));
            if (attribute == null)
            {
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Error,
                    EdcCheckCategory.UI,
                    RuleName,
                    $"UI控制器 {type.Name} 缺少 ResourceKeyAttribute",
                    EdcCheckerUtility.GetScript(type));
                continue;
            }

            if (string.IsNullOrWhiteSpace(attribute.Key))
            {
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Error,
                    EdcCheckCategory.UI,
                    RuleName,
                    $"UI控制器 {type.Name} 的 ResourceKey 为空",
                    EdcCheckerUtility.GetScript(type));
                continue;
            }

            usedKeys.Add(attribute.Key);

            if (!panelKeys.Contains(attribute.Key))
            {
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Error,
                    EdcCheckCategory.UI,
                    RuleName,
                    $"UI控制器 {type.Name} 使用的 ResourceKey 不存在于 UI 配置中：{attribute.Key}",
                    EdcCheckerUtility.GetScript(type));
            }
        }

        foreach (var panelKey in panelKeys)
        {
            if (!usedKeys.Contains(panelKey))
            {
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Warning,
                    EdcCheckCategory.UI,
                    RuleName,
                    $"UI配置中存在面板 {panelKey}，但没有找到使用该 ResourceKey 的 UI控制器");
            }
        }
    }
}

