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


internal sealed class AddressablesProjectRule : IEdcCheckRule
{
    public string RuleName => "Addressables基础检查";

    public IEnumerable<EdcCheckResult> Check(EdcCheckContext context)
    {
        if (context.AddressableSettings == null)
        {
            yield return new EdcCheckResult(
                EdcCheckSeverity.Error,
                EdcCheckCategory.Addressables,
                RuleName,
                "项目中没有找到 AddressableAssetSettings，资源系统无法工作");
            yield break;
        }

        if (context.AddressableEntries.Count == 0)
        {
            yield return new EdcCheckResult(
                EdcCheckSeverity.Error,
                EdcCheckCategory.Addressables,
                RuleName,
                "Addressables 中没有任何资源条目");
            yield break;
        }

        foreach (var duplicateGroup in context.AddressableEntries
                     .Where(item => !string.IsNullOrEmpty(item.address))
                     .GroupBy(item => item.address)
                     .Where(group => group.Count() > 1))
        {
            var firstEntry = duplicateGroup.First();
            yield return new EdcCheckResult(
                EdcCheckSeverity.Error,
                EdcCheckCategory.Addressables,
                RuleName,
                $"Addressables 地址重复：{duplicateGroup.Key}",
                AssetDatabase.LoadMainAssetAtPath(firstEntry.AssetPath),
                firstEntry.AssetPath);
        }

        foreach (var entry in context.AddressableEntries)
        {
            if (string.IsNullOrEmpty(entry.address))
            {
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Error,
                    EdcCheckCategory.Addressables,
                    RuleName,
                    "存在空 Addressables 地址",
                    AssetDatabase.LoadMainAssetAtPath(entry.AssetPath),
                    entry.AssetPath);
            }

            if (!ShouldCheckAssetPath(entry))
            {
                continue;
            }

            var asset = AssetDatabase.LoadMainAssetAtPath(entry.AssetPath);
            if (asset == null)
            {
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Error,
                    EdcCheckCategory.Addressables,
                    RuleName,
                    $"Addressables 地址 {entry.address} 指向的资源不存在：{entry.AssetPath}",
                    null,
                    entry.AssetPath);
            }
        }
    }

    private static bool ShouldCheckAssetPath(AddressableAssetEntry entry)
    {
        if (string.IsNullOrEmpty(entry.AssetPath))
        {
            return false;
        }

        return entry.AssetPath.StartsWith("Assets/", StringComparison.Ordinal);
    }
}

