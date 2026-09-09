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


internal sealed class UiSettingRule : IEdcCheckRule
{
    public string RuleName => "UI配置检查";

    public IEnumerable<EdcCheckResult> Check(EdcCheckContext context)
    {
        foreach (var result in CheckSetting<ViewSetting>(context, EdcCheckerPaths.ViewSettingPath, "View"))
        {
            yield return result;
        }

        foreach (var result in CheckSetting<PersistentViewSetting>(context, EdcCheckerPaths.PersistentViewSettingPath, "PersistentView"))
        {
            yield return result;
        }

        foreach (var result in CheckSetting<WindowSetting>(context, EdcCheckerPaths.WindowSettingPath, "Window"))
        {
            yield return result;
        }
    }

    internal static HashSet<string> GetAllPanelKeys()
    {
        var keys = new HashSet<string>(StringComparer.Ordinal);
        AddPanelKeys<ViewSetting>(keys, EdcCheckerPaths.ViewSettingPath);
        AddPanelKeys<PersistentViewSetting>(keys, EdcCheckerPaths.PersistentViewSettingPath);
        AddPanelKeys<WindowSetting>(keys, EdcCheckerPaths.WindowSettingPath);
        return keys;
    }

    private static IEnumerable<EdcCheckResult> CheckSetting<T>(
        EdcCheckContext context,
        string settingPath,
        string uiTypeName) where T : UIPrefabSetting
    {
        var setting = AssetDatabase.LoadAssetAtPath<T>(settingPath);
        if (setting == null)
        {
            yield return new EdcCheckResult(
                EdcCheckSeverity.Error,
                EdcCheckCategory.UI,
                "UI配置检查",
                $"{uiTypeName} 配置资产不存在：{settingPath}",
                null,
                settingPath);
            yield break;
        }

        if (setting.panelList == null)
        {
            yield return new EdcCheckResult(
                EdcCheckSeverity.Error,
                EdcCheckCategory.UI,
                "UI配置检查",
                $"{uiTypeName} 配置的 panelList 为空，会导致初始化失败",
                setting,
                settingPath);
            yield break;
        }

        if (setting.panelList.Count == 0)
        {
            yield return new EdcCheckResult(
                EdcCheckSeverity.Info,
                EdcCheckCategory.UI,
                "UI配置检查",
                $"{uiTypeName} 当前没有配置任何面板",
                setting,
                settingPath);
            yield break;
        }

        var panelNames = new HashSet<string>(StringComparer.Ordinal);
        var prefabKeys = new HashSet<string>(StringComparer.Ordinal);

        foreach (var entry in setting.panelList)
        {
            if (entry == null)
            {
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Error,
                    EdcCheckCategory.UI,
                    "UI配置检查",
                    $"{uiTypeName} 配置中存在空条目",
                    setting,
                    settingPath);
                continue;
            }

            if (string.IsNullOrWhiteSpace(entry.name))
            {
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Error,
                    EdcCheckCategory.UI,
                    "UI配置检查",
                    $"{uiTypeName} 配置中存在空面板名",
                    setting,
                    settingPath);
            }
            else if (!panelNames.Add(entry.name))
            {
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Error,
                    EdcCheckCategory.UI,
                    "UI配置检查",
                    $"{uiTypeName} 面板名重复：{entry.name}",
                    setting,
                    settingPath);
            }

            if (entry.info == null)
            {
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Error,
                    EdcCheckCategory.UI,
                    "UI配置检查",
                    $"{uiTypeName} 面板 {entry.name} 的 UIPrefabInfo 为空",
                    setting,
                    settingPath);
                continue;
            }

            if (string.IsNullOrWhiteSpace(entry.info.prefab))
            {
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Error,
                    EdcCheckCategory.UI,
                    "UI配置检查",
                    $"{uiTypeName} 面板 {entry.name} 没有填写预制体 Addressables 地址",
                    setting,
                    settingPath);
                continue;
            }

            if (!prefabKeys.Add(entry.info.prefab))
            {
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Warning,
                    EdcCheckCategory.UI,
                    "UI配置检查",
                    $"{uiTypeName} 中多个面板使用了同一个预制体地址：{entry.info.prefab}",
                    setting,
                    settingPath);
            }

            if (!context.HasAddress(entry.info.prefab))
            {
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Error,
                    EdcCheckCategory.UI,
                    "UI配置检查",
                    $"{uiTypeName} 面板 {entry.name} 引用的预制体地址不存在：{entry.info.prefab}",
                    setting,
                    settingPath);
                continue;
            }

            var addressPath = context.GetAddressPath(entry.info.prefab);
            var assetType = AssetDatabase.GetMainAssetTypeAtPath(addressPath);
            if (assetType != null && !typeof(GameObject).IsAssignableFrom(assetType))
            {
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Error,
                    EdcCheckCategory.UI,
                    "UI配置检查",
                    $"{uiTypeName} 面板 {entry.name} 的地址 {entry.info.prefab} 指向的不是预制体",
                    context.GetAddressContext(entry.info.prefab),
                    addressPath);
            }
        }
    }

    private static void AddPanelKeys<T>(HashSet<string> keys, string settingPath) where T : UIPrefabSetting
    {
        var setting = AssetDatabase.LoadAssetAtPath<T>(settingPath);
        if (setting?.panelList == null)
        {
            return;
        }

        foreach (var entry in setting.panelList)
        {
            if (!string.IsNullOrWhiteSpace(entry?.name))
            {
                keys.Add(entry.name);
            }
        }
    }
}

