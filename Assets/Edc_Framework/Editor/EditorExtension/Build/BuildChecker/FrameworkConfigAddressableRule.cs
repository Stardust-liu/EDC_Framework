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


internal sealed class FrameworkConfigAddressableRule : IEdcCheckRule
{
    public string RuleName => "框架配置资源检查";

    private static readonly Dictionary<string, string> RequiredAddresses = new()
    {
        { "LocalizationFontSetting", EdcCheckerPaths.LocalizationFontSettingPath },
        { "ObjectPoolSetting", EdcCheckerPaths.ObjectPoolSettingPath },
        { "PersistentViewSetting", EdcCheckerPaths.PersistentViewSettingPath },
        { "ViewSetting", EdcCheckerPaths.ViewSettingPath },
        { "WindowSetting", EdcCheckerPaths.WindowSettingPath },
        { "RedDotTreeSetting", EdcCheckerPaths.RedDotTreeSettingPath },
        { "InputSetting", EdcCheckerPaths.InputSettingPath },
        { "SceneResourcesSetting", EdcCheckerPaths.SceneResourcesSettingPath },
    };

    public IEnumerable<EdcCheckResult> Check(EdcCheckContext context)
    {
        foreach (var item in RequiredAddresses)
        {
            var asset = AssetDatabase.LoadMainAssetAtPath(item.Value);
            if (asset == null)
            {
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Error,
                    EdcCheckCategory.FrameworkConfig,
                    RuleName,
                    $"框架配置资产不存在：{item.Value}",
                    null,
                    item.Value);
                continue;
            }

            if (!context.HasAddress(item.Key))
            {
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Error,
                    EdcCheckCategory.FrameworkConfig,
                    RuleName,
                    $"框架配置资产没有配置 Addressables 地址：{item.Key}",
                    asset,
                    item.Value);
                continue;
            }

            var addressPath = context.GetAddressPath(item.Key);
            if (!string.Equals(addressPath, item.Value, StringComparison.Ordinal))
            {
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Warning,
                    EdcCheckCategory.FrameworkConfig,
                    RuleName,
                    $"Addressables 地址 {item.Key} 指向 {addressPath}，与框架默认配置路径不一致",
                    context.GetAddressContext(item.Key),
                    addressPath);
            }
        }
    }
}

