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

internal sealed class SceneResourceRule : IEdcCheckRule
{
    public string RuleName => "场景资源配置检查";

    public IEnumerable<EdcCheckResult> Check(EdcCheckContext context)
    {
        var setting = AssetDatabase.LoadAssetAtPath<SceneResourcesSetting>(EdcCheckerPaths.SceneResourcesSettingPath);
        if (setting == null)
        {
            yield return new EdcCheckResult(
                EdcCheckSeverity.Error,
                EdcCheckCategory.Scene,
                RuleName,
                $"场景资源配置不存在：{EdcCheckerPaths.SceneResourcesSettingPath}",
                null,
                EdcCheckerPaths.SceneResourcesSettingPath);
            yield break;
        }

        if (setting.keyValuePairs == null)
        {
            yield return new EdcCheckResult(
                EdcCheckSeverity.Error,
                EdcCheckCategory.Scene,
                RuleName,
                "SceneResourcesSetting.keyValuePairs 为空，运行时查询场景资源会失败",
                setting,
                EdcCheckerPaths.SceneResourcesSettingPath);
            yield break;
        }

        if (setting.keyValuePairs.Count == 0)
        {
            yield return new EdcCheckResult(
                EdcCheckSeverity.Info,
                EdcCheckCategory.Scene,
                RuleName,
                "当前没有配置场景资源",
                setting,
                EdcCheckerPaths.SceneResourcesSettingPath);
            yield break;
        }

        foreach (var item in setting.keyValuePairs)
        {
            if (string.IsNullOrWhiteSpace(item.Key))
            {
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Error,
                    EdcCheckCategory.Scene,
                    RuleName,
                    "SceneResourcesSetting 中存在空场景名",
                    setting,
                    EdcCheckerPaths.SceneResourcesSettingPath);
            }

            if (item.Value == null)
            {
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Error,
                    EdcCheckCategory.Scene,
                    RuleName,
                    $"场景 {item.Key} 没有关联 SceneResourceConfig",
                    setting,
                    EdcCheckerPaths.SceneResourcesSettingPath);
                continue;
            }

            foreach (var result in CheckSceneConfig(context, item.Key, item.Value))
            {
                yield return result;
            }
        }
    }

    private IEnumerable<EdcCheckResult> CheckSceneConfig(EdcCheckContext context, string sceneName, SceneResourceConfig config)
    {
        var configPath = AssetDatabase.GetAssetPath(config);

        if (config.addressableLabels == null)
        {
            yield return new EdcCheckResult(
                EdcCheckSeverity.Error,
                EdcCheckCategory.Scene,
                RuleName,
                $"场景 {sceneName} 的 addressableLabels 为 null，运行时 Load 会失败",
                config,
                configPath);
        }
        else
        {
            foreach (var label in config.addressableLabels)
            {
                if (string.IsNullOrWhiteSpace(label))
                {
                    yield return new EdcCheckResult(
                        EdcCheckSeverity.Error,
                        EdcCheckCategory.Scene,
                        RuleName,
                        $"场景 {sceneName} 的资源 Label 为空",
                        config,
                        configPath);
                    continue;
                }

                if (!context.HasUsedLabel(label))
                {
                    yield return new EdcCheckResult(
                        EdcCheckSeverity.Error,
                        EdcCheckCategory.Scene,
                        RuleName,
                        $"场景 {sceneName} 引用的 Label 不存在或没有资源使用：{label}",
                        config,
                        configPath);
                }
            }
        }

        if (config.addressables == null)
        {
            yield return new EdcCheckResult(
                EdcCheckSeverity.Error,
                EdcCheckCategory.Scene,
                RuleName,
                $"场景 {sceneName} 的 addressables 为 null，运行时 Load 会失败",
                config,
                configPath);
        }
        else
        {
            foreach (var address in config.addressables)
            {
                if (string.IsNullOrWhiteSpace(address))
                {
                    yield return new EdcCheckResult(
                        EdcCheckSeverity.Error,
                        EdcCheckCategory.Scene,
                        RuleName,
                        $"场景 {sceneName} 的资源地址为空",
                        config,
                        configPath);
                    continue;
                }

                if (!context.HasAddress(address))
                {
                    yield return new EdcCheckResult(
                        EdcCheckSeverity.Error,
                        EdcCheckCategory.Scene,
                        RuleName,
                        $"场景 {sceneName} 引用的资源地址不存在：{address}",
                        config,
                        configPath);
                }
            }
        }
    }
}

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

internal sealed class ObjectPoolResourceKeyRule : IEdcCheckRule
{
    public string RuleName => "对象池ResourceKey检查";

    public IEnumerable<EdcCheckResult> Check(EdcCheckContext context)
    {
        var poolNames = ObjectPoolRule.GetAllPoolNames();
        var poolTypes = TypeCache.GetTypesDerivedFrom<BasePool>();

        foreach (var type in poolTypes)
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
                    EdcCheckCategory.ObjectPool,
                    RuleName,
                    $"对象池类 {type.Name} 缺少 ResourceKeyAttribute",
                    EdcCheckerUtility.GetScript(type));
                continue;
            }

            if (string.IsNullOrWhiteSpace(attribute.Key))
            {
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Error,
                    EdcCheckCategory.ObjectPool,
                    RuleName,
                    $"对象池类 {type.Name} 的 ResourceKey 为空",
                    EdcCheckerUtility.GetScript(type));
                continue;
            }

            if (!poolNames.Contains(attribute.Key))
            {
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Error,
                    EdcCheckCategory.ObjectPool,
                    RuleName,
                    $"对象池类 {type.Name} 使用的 ResourceKey 不存在于 ObjectPoolSetting 中：{attribute.Key}",
                    EdcCheckerUtility.GetScript(type));
            }
        }
    }
}

internal sealed class BuildSettingRule : IEdcCheckRule
{
    public string RuleName => "构建配置检查";

    public IEnumerable<EdcCheckResult> Check(EdcCheckContext context)
    {
        var enabledScenes = EditorBuildSettings.scenes.Where(item => item.enabled).ToList();
        if (enabledScenes.Count == 0)
        {
            yield return new EdcCheckResult(
                EdcCheckSeverity.Error,
                EdcCheckCategory.Build,
                RuleName,
                "Build Settings 中没有启用任何场景，可能会导致构建或运行失败");
        }

        var mainScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EdcCheckerPaths.MainScenePath);
        if (mainScene == null)
        {
            yield return new EdcCheckResult(
                EdcCheckSeverity.Warning,
                EdcCheckCategory.Build,
                RuleName,
                $"没有找到框架默认启动场景：{EdcCheckerPaths.MainScenePath}",
                null,
                EdcCheckerPaths.MainScenePath);
            yield break;
        }

        var hasMainScene = EditorBuildSettings.scenes.Any(item =>
            item.enabled && string.Equals(item.path, EdcCheckerPaths.MainScenePath, StringComparison.Ordinal));

        if (!hasMainScene)
        {
            yield return new EdcCheckResult(
                EdcCheckSeverity.Warning,
                EdcCheckCategory.Build,
                RuleName,
                "框架默认启动场景没有加入 Build Settings 或未启用",
                mainScene,
                EdcCheckerPaths.MainScenePath);
        }
    }
}
