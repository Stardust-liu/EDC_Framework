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


internal interface IEdcCheckRule
{
    string RuleName { get; }
    IEnumerable<EdcCheckResult> Check(EdcCheckContext context);
}

internal static class EdcCheckRunner
{
    private static readonly List<IEdcCheckRule> Rules = new()
    {
        new AddressablesProjectRule(),
        new FrameworkConfigAddressableRule(),
        new LabelConfigManagerRule(),
        new UiSettingRule(),
        new UiControlResourceKeyRule(),
        new SceneResourceRule(),
        new ObjectPoolRule(),
        new ObjectPoolResourceKeyRule(),
        new ArchiveKeyRule(),
        new BuildSettingRule(),
    };

    internal static List<EdcCheckResult> CheckAll()
    {
        var context = EdcCheckContext.Create();
        var results = new List<EdcCheckResult>();

        foreach (var rule in Rules)
        {
            try
            {
                results.AddRange(rule.Check(context));
            }
            catch (Exception exception)
            {
                results.Add(new EdcCheckResult(
                    EdcCheckSeverity.Error,
                    EdcCheckCategory.Addressables,
                    rule.RuleName,
                    $"检查规则执行失败：{exception.Message}"));
            }
        }

        if (results.Count == 0)
        {
            results.Add(new EdcCheckResult(
                EdcCheckSeverity.Info,
                EdcCheckCategory.Addressables,
                "检查完成",
                "未发现问题"));
        }

        return results
            .OrderByDescending(item => item.Severity)
            .ThenBy(item => item.Category)
            .ThenBy(item => item.RuleName)
            .ToList();
    }
}

internal sealed class EdcCheckContext
{
    internal AddressableAssetSettings AddressableSettings { get; private set; }
    internal List<AddressableAssetEntry> AddressableEntries { get; } = new();
    internal HashSet<string> DefinedLabels { get; } = new(StringComparer.Ordinal);
    internal HashSet<string> UsedLabels { get; } = new(StringComparer.Ordinal);

    private readonly Dictionary<string, List<AddressableAssetEntry>> entriesByAddress = new(StringComparer.Ordinal);

    private EdcCheckContext()
    {
    }

    internal static EdcCheckContext Create()
    {
        var context = new EdcCheckContext
        {
            AddressableSettings = AddressableAssetSettingsDefaultObject.SettingsExists
                ? AddressableAssetSettingsDefaultObject.Settings
                : null,
        };

        if (context.AddressableSettings == null)
        {
            return context;
        }

        foreach (var label in context.AddressableSettings.GetLabels())
        {
            if (!string.IsNullOrEmpty(label))
            {
                context.DefinedLabels.Add(label);
            }
        }

        foreach (var group in context.AddressableSettings.groups)
        {
            if (group == null)
            {
                continue;
            }

            foreach (var entry in group.entries)
            {
                context.AddEntry(entry);
            }
        }

        return context;
    }

    internal bool HasAddress(string address)
    {
        return !string.IsNullOrEmpty(address) && entriesByAddress.ContainsKey(address);
    }

    internal bool HasUsedLabel(string label)
    {
        return !string.IsNullOrEmpty(label) && UsedLabels.Contains(label);
    }

    internal bool HasDefinedLabel(string label)
    {
        return !string.IsNullOrEmpty(label) && DefinedLabels.Contains(label);
    }

    internal IReadOnlyList<AddressableAssetEntry> GetEntries(string address)
    {
        return entriesByAddress.TryGetValue(address, out var entries) ? entries : Array.Empty<AddressableAssetEntry>();
    }

    internal AddressableAssetEntry GetFirstEntry(string address)
    {
        return entriesByAddress.TryGetValue(address, out var entries) && entries.Count > 0 ? entries[0] : null;
    }

    internal Object GetAddressContext(string address)
    {
        var entry = GetFirstEntry(address);
        return entry == null ? null : AssetDatabase.LoadMainAssetAtPath(entry.AssetPath);
    }

    internal string GetAddressPath(string address)
    {
        return GetFirstEntry(address)?.AssetPath ?? string.Empty;
    }

    private void AddEntry(AddressableAssetEntry entry)
    {
        if (entry == null)
        {
            return;
        }

        AddressableEntries.Add(entry);

        if (!string.IsNullOrEmpty(entry.address))
        {
            if (!entriesByAddress.TryGetValue(entry.address, out var entries))
            {
                entries = new List<AddressableAssetEntry>();
                entriesByAddress.Add(entry.address, entries);
            }

            entries.Add(entry);
        }

        foreach (var label in entry.labels)
        {
            if (!string.IsNullOrEmpty(label))
            {
                UsedLabels.Add(label);
            }
        }
    }
}

internal static class EdcCheckerPaths
{
    internal const string ViewSettingPath = "Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/UI/ViewSetting.asset";
    internal const string PersistentViewSettingPath = "Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/UI/PersistentViewSetting.asset";
    internal const string WindowSettingPath = "Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/UI/WindowSetting.asset";
    internal const string SceneResourcesSettingPath = "Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/Scene/SceneResourcesSetting.asset";
    internal const string ObjectPoolSettingPath = "Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/ObjectPool/ObjectPoolSetting.asset";
    internal const string LocalizationFontSettingPath = "Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/Localization/LocalizationFontSetting.asset";
    internal const string InputSettingPath = "Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/Input/InputSetting.asset";
    internal const string RedDotTreeSettingPath = "Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/RedDotTree/RedDotTreeSetting.asset";
    internal const string MainScenePath = "Assets/Edc_Framework/Scenes/MainScene.unity";
}

internal static class EdcCheckerUtility
{
    internal static MonoScript GetScript(Type type)
    {
        var scriptGuids = AssetDatabase.FindAssets($"{type.Name} t:MonoScript");
        foreach (var guid in scriptGuids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var script = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
            if (script != null && script.GetClass() == type)
            {
                return script;
            }
        }

        return null;
    }
}

