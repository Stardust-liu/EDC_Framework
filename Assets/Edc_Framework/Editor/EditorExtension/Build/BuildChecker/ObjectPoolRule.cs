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


internal sealed class ObjectPoolRule : IEdcCheckRule
{
    public string RuleName => "对象池资源配置检查";

    public IEnumerable<EdcCheckResult> Check(EdcCheckContext context)
    {
        var setting = AssetDatabase.LoadAssetAtPath<ObjectPoolSetting>(EdcCheckerPaths.ObjectPoolSettingPath);
        if (setting == null)
        {
            yield return new EdcCheckResult(
                EdcCheckSeverity.Error,
                EdcCheckCategory.ObjectPool,
                RuleName,
                $"对象池配置不存在：{EdcCheckerPaths.ObjectPoolSettingPath}",
                null,
                EdcCheckerPaths.ObjectPoolSettingPath);
            yield break;
        }

        if (setting.prefabList == null)
        {
            yield return new EdcCheckResult(
                EdcCheckSeverity.Error,
                EdcCheckCategory.ObjectPool,
                RuleName,
                "ObjectPoolSetting.prefabList 为空，运行时初始化会失败",
                setting,
                EdcCheckerPaths.ObjectPoolSettingPath);
            yield break;
        }

        if (setting.prefabList.Count == 0)
        {
            yield return new EdcCheckResult(
                EdcCheckSeverity.Info,
                EdcCheckCategory.ObjectPool,
                RuleName,
                "当前没有配置对象池资源",
                setting,
                EdcCheckerPaths.ObjectPoolSettingPath);
            yield break;
        }

        var poolNames = new HashSet<string>(StringComparer.Ordinal);
        var keyNames = new HashSet<string>(StringComparer.Ordinal);

        foreach (var entry in setting.prefabList)
        {
            if (entry == null)
            {
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Error,
                    EdcCheckCategory.ObjectPool,
                    RuleName,
                    "对象池配置中存在空条目",
                    setting,
                    EdcCheckerPaths.ObjectPoolSettingPath);
                continue;
            }

            if (string.IsNullOrWhiteSpace(entry.name))
            {
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Error,
                    EdcCheckCategory.ObjectPool,
                    RuleName,
                    "对象池配置中存在空对象池名",
                    setting,
                    EdcCheckerPaths.ObjectPoolSettingPath);
            }
            else if (!poolNames.Add(entry.name))
            {
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Error,
                    EdcCheckCategory.ObjectPool,
                    RuleName,
                    $"对象池名称重复：{entry.name}",
                    setting,
                    EdcCheckerPaths.ObjectPoolSettingPath);
            }

            if (entry.info == null)
            {
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Error,
                    EdcCheckCategory.ObjectPool,
                    RuleName,
                    $"对象池 {entry.name} 的 PoolInfo 为空",
                    setting,
                    EdcCheckerPaths.ObjectPoolSettingPath);
                continue;
            }

            if (string.IsNullOrWhiteSpace(entry.info.keyName))
            {
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Error,
                    EdcCheckCategory.ObjectPool,
                    RuleName,
                    $"对象池 {entry.name} 没有填写资源地址",
                    setting,
                    EdcCheckerPaths.ObjectPoolSettingPath);
                continue;
            }

            if (!keyNames.Add(entry.info.keyName))
            {
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Warning,
                    EdcCheckCategory.ObjectPool,
                    RuleName,
                    $"多个对象池使用了同一个资源地址：{entry.info.keyName}",
                    setting,
                    EdcCheckerPaths.ObjectPoolSettingPath);
            }

            if (!context.HasAddress(entry.info.keyName))
            {
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Error,
                    EdcCheckCategory.ObjectPool,
                    RuleName,
                    $"对象池 {entry.name} 引用的资源地址不存在：{entry.info.keyName}",
                    setting,
                    EdcCheckerPaths.ObjectPoolSettingPath);
                continue;
            }

            var addressPath = context.GetAddressPath(entry.info.keyName);
            var assetType = AssetDatabase.GetMainAssetTypeAtPath(addressPath);
            if (assetType != null && !typeof(GameObject).IsAssignableFrom(assetType))
            {
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Error,
                    EdcCheckCategory.ObjectPool,
                    RuleName,
                    $"对象池 {entry.name} 的资源地址 {entry.info.keyName} 指向的不是预制体",
                    context.GetAddressContext(entry.info.keyName),
                    addressPath);
            }
        }
    }

    internal static HashSet<string> GetAllPoolNames()
    {
        var keys = new HashSet<string>(StringComparer.Ordinal);
        var setting = AssetDatabase.LoadAssetAtPath<ObjectPoolSetting>(EdcCheckerPaths.ObjectPoolSettingPath);
        if (setting?.prefabList == null)
        {
            return keys;
        }

        foreach (var entry in setting.prefabList)
        {
            if (!string.IsNullOrWhiteSpace(entry?.name))
            {
                keys.Add(entry.name);
            }
        }

        return keys;
    }
}

