using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build.Reporting;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildSettingTool", menuName = "创建.Assets文件/FrameworkTool/BuildSettingTool")]
public class BuildSettingTool : ScriptableObject
{
    public const string AssetPath = "Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/Build/BuildSettingTool.asset";
    private const string PlatformResourceGroup = "渠道平台资源模块";
    private const string PlatformResourceRootGroup = PlatformResourceGroup + "/PlatformResourceRoot";
    private const string PlatformImportRootGroup = PlatformResourceGroup + "/PlatformImportRoot";

    [PropertyOrder(0)]
    [FoldoutGroup(PlatformResourceGroup)]
    [OnInspectorGUI]
    private void DrawPlatformResourceInfo()
    {
        EditorGUILayout.HelpBox("用于管理不同渠道或平台的 SDK、代码、配置和资源。应用当前配置时，会从外部存放地址复制到工程接入地址，并同步宏定义。", MessageType.Info);
    }

    [PropertyOrder(1)]
    [FoldoutGroup(PlatformResourceGroup)]
    [HorizontalGroup(PlatformResourceRootGroup)]
    [LabelText("平台资源外部存放地址")]
    [Tooltip("存放各渠道或平台资源的目录")]
    public string platformModulesRootPath = "PlatformModules";

    [PropertyOrder(1)]
    [FoldoutGroup(PlatformResourceGroup)]
    [HorizontalGroup(PlatformResourceRootGroup, Width = 70)]
    [Button("打开目录")]
    private void OpenPlatformModulesRootFolder()
    {
        OpenFolder(GetFullPath(platformModulesRootPath));
    }

    [PropertyOrder(1)]
    [FoldoutGroup(PlatformResourceGroup)]
    [HorizontalGroup(PlatformImportRootGroup)]
    [LabelText("平台资源工程存放地址")]
    [Tooltip("平台资源复制进 Unity 工程后所在的位置")]
    public string platformImportRootPath = "Assets/Edc_Framework/Script/_Platform";

    [PropertyOrder(1)]
    [FoldoutGroup(PlatformResourceGroup)]
    [HorizontalGroup(PlatformImportRootGroup, Width = 70)]
    [Button("打开目录")]
    private void OpenPlatformImportRootFolder()
    {
        OpenFolder(GetFullPath(platformImportRootPath));
    }


    [PropertyOrder(10)]
    [FoldoutGroup("当前构建设置")]
    [ValueDropdown(nameof(GetProfileDropdown))]
    [LabelText("打包配置")]
    public string selectedProfileId = "Dev";

    [PropertyOrder(10)]
    [FoldoutGroup("当前构建设置")]
    [LabelText("应用版本")]
    [Tooltip("公共基础版本号。最终写入 PlayerSettings 的版本号会由当前 Profile 决定是否追加后缀。")]
    public string baseVersion = "0.1";

    [PropertyOrder(10)]
    [FoldoutGroup("当前构建设置")]
    [OnInspectorGUI]
    private void DrawCurrentProfileDisplayItems()
    {
        if (!TryGetSelectedProfileSilently(out var profile))
        {
            EditorGUILayout.HelpBox("未找到打包配置", MessageType.Warning);
            return;
        }

        var displayContext = new BuildProfileDisplayContext(
            baseVersion,
            profile.GetPlayerVersion(baseVersion),
            GetBuildOutputName(profile));

        using (new EditorGUI.DisabledScope(true))
        {
            var displayItems = profile.GetDisplayItems(displayContext);
            if (displayItems == null)
            {
                return;
            }

            foreach (var item in displayItems)
            {
                EditorGUILayout.TextField(item.Name, item.Value ?? string.Empty);
            }
        }
    }

    [PropertyOrder(10)]
    [FoldoutGroup("当前构建设置")]
    [Button("刷新打包配置列表", ButtonSizes.Medium)]
    private void RefreshProfiles()
    {
        BuildProfileRegistry.Refresh();
        if (TryGetSelectedProfile(out var profile))
        {
            Debug.Log($"打包配置列表已刷新，当前配置：{profile.DisplayName}");
        }
    }

    [PropertyOrder(10)]
    [FoldoutGroup("当前构建设置")]
    [Button("应用当前配置", ButtonSizes.Large), GUIColor(0.5f, 0.8f, 1f)]
    private void ApplyCurrentProfile()
    {
        if (!TryGetSelectedProfile(out var profile))
        {
            return;
        }

        if (ApplyProfile(profile))
        {
            AssetDatabase.Refresh();
        }
    }

    [PropertyOrder(20)]
    [FoldoutGroup("最终打包"), LabelText("打包前框架检查")]
    public bool runFrameworkCheckBeforeBuild = true;

    [PropertyOrder(20)]
    [FoldoutGroup("最终打包"), LabelText("检查错误时停止打包")]
    public bool stopBuildWhenCheckError = true;

    [PropertyOrder(20)]
    [FoldoutGroup("最终打包"), LabelText("打包前构建Addressables")]
    public bool buildAddressablesBeforeBuild = true;

    [PropertyOrder(20)]
    [FoldoutGroup("最终打包"), LabelText("输出名包含版本号")]
    public bool appendVersionToBuildName = true;

    [PropertyOrder(20)]
    [FoldoutGroup("最终打包")]
    [HorizontalGroup("最终打包/OutputPath")]
    [LabelText("输出目录")]
    public string buildOutputRootPath = "Builds";

    [PropertyOrder(20)]
    [FoldoutGroup("最终打包")]
    [HorizontalGroup("最终打包/OutputPath", Width = 70)]
    [Button("打开目录")]
    private void OpenBuildOutputFolder()
    {
        OpenFolder(GetFullPath(buildOutputRootPath));
    }

    [PropertyOrder(20)]
    [FoldoutGroup("最终打包")]
    [Button("打包前检查", ButtonSizes.Large), GUIColor(1f, 0.85f, 0.45f)]
    private void RunPreBuildCheck()
    {
        RunFrameworkCheck();
    }

    [PropertyOrder(20)]
    [FoldoutGroup("最终打包")]
    [Button("打包当前配置", ButtonSizes.Large), GUIColor(0.5f, 0.8f, 1f)]
    private void BuildCurrentProfile()
    {
        if (EditorApplication.isCompiling)
        {
            Debug.LogError("Unity 正在编译脚本，已取消打包");
            return;
        }

        if (!TryGetSelectedProfile(out var profile))
        {
            return;
        }

        if (!ApplyProfile(profile))
        {
            return;
        }

        if (EditorApplication.isCompiling)
        {
            Debug.LogWarning("应用打包配置后 Unity 正在编译脚本，请等待编译完成后重新点击打包");
            return;
        }

        if (runFrameworkCheckBeforeBuild)
        {
            var checkResults = RunFrameworkCheck();
            if (stopBuildWhenCheckError && EdcFrameworkChecker.HasError(checkResults))
            {
                Debug.LogError("框架检查存在错误，已取消打包");
                return;
            }
        }

        if (buildAddressablesBeforeBuild && !BuildAddressablesContent())
        {
            return;
        }

        var scenePaths = GetEnabledScenePaths();
        if (scenePaths.Length == 0)
        {
            Debug.LogError("Build Settings 中没有启用任何场景，已取消打包");
            return;
        }

        var outputPath = GetBuildOutputPath(profile);
        EnsureBuildOutputFolder(outputPath, profile.BuildTarget);

        var report = BuildPipeline.BuildPlayer(scenePaths, outputPath, profile.BuildTarget, GetBuildOptions(profile));
        LogBuildReport(report, outputPath);
    }


    private void OnValidate()
    {
        if (string.IsNullOrWhiteSpace(selectedProfileId))
        {
            selectedProfileId = "Dev";
        }

        if (string.IsNullOrWhiteSpace(baseVersion))
        {
            baseVersion = "0.1";
        }
    }

    private IEnumerable<string> GetProfileDropdown()
    {
        foreach (var profile in BuildProfileRegistry.Profiles)
        {
            yield return profile.ProfileId;
        }
    }

    private bool TryGetSelectedProfile(out BuildProfile profile)
    {
        if (BuildProfileRegistry.TryGetProfile(selectedProfileId, out profile))
        {
            return true;
        }

        profile = BuildProfileRegistry.GetDefaultProfile();
        if (profile != null)
        {
            Debug.LogWarning($"未找到打包配置：{selectedProfileId}，已切换到默认配置：{profile.DisplayName}");
            selectedProfileId = profile.ProfileId;
            return true;
        }

        Debug.LogError("没有找到任何打包配置，请在 Build/Profile 目录下创建继承 BuildProfile 的类");
        return false;
    }

    private bool TryGetSelectedProfileSilently(out BuildProfile profile)
    {
        if (BuildProfileRegistry.TryGetProfile(selectedProfileId, out profile))
        {
            return true;
        }

        profile = BuildProfileRegistry.GetDefaultProfile();
        return profile != null;
    }

    private bool ApplyProfile(BuildProfile profile)
    {
        if (!ApplyBuildProjectInfo())
        {
            return false;
        }

        if (!SwitchBuildTarget(profile))
        {
            return false;
        }

        PlayerSettings.bundleVersion = profile.GetPlayerVersion(baseVersion);
        if (!profile.ApplyProfileSettings())
        {
            return false;
        }

        RemoveUnselectedPlatformModules(profile);
        RemoveUnselectedDefineSymbols(profile);

        if (!CopyPlatformModules(profile))
        {
            AssetDatabase.Refresh();
            return false;
        }

        AddDefineSymbols(profile);
        Debug.Log($"已应用打包配置：{profile.DisplayName}，版本：{PlayerSettings.bundleVersion}");
        return true;
    }

    private static bool SwitchBuildTarget(BuildProfile profile)
    {
        if (profile.BuildTargetGroup == BuildTargetGroup.Unknown)
        {
            Debug.LogError($"打包配置 {profile.DisplayName} 的构建平台组无效");
            return false;
        }

        if (EditorUserBuildSettings.activeBuildTarget == profile.BuildTarget &&
            EditorUserBuildSettings.selectedBuildTargetGroup == profile.BuildTargetGroup)
        {
            return true;
        }

        if (EditorUserBuildSettings.SwitchActiveBuildTarget(profile.BuildTargetGroup, profile.BuildTarget))
        {
            Debug.Log($"已切换构建平台：{profile.BuildTargetGroup} / {profile.BuildTarget}");
            return true;
        }

        Debug.LogError($"切换构建平台失败：{profile.BuildTargetGroup} / {profile.BuildTarget}");
        return false;
    }

    private static List<EdcCheckResult> RunFrameworkCheck()
    {
        var results = EdcFrameworkChecker.CheckAll();
        EdcFrameworkChecker.LogSummary(results);
        return results;
    }

    private static bool BuildAddressablesContent()
    {
        if (!AddressableAssetSettingsDefaultObject.SettingsExists || AddressableAssetSettingsDefaultObject.Settings == null)
        {
            Debug.LogError("项目中没有找到 AddressableAssetSettings，已取消构建 Addressables");
            return false;
        }

        AddressableAssetSettings.BuildPlayerContent(out AddressablesPlayerBuildResult result);
        if (result == null)
        {
            Debug.LogError("Addressables 构建失败：没有返回构建结果");
            return false;
        }

        if (!string.IsNullOrEmpty(result.Error))
        {
            Debug.LogError($"Addressables 构建失败：{result.Error}");
            return false;
        }

        Debug.Log($"Addressables 构建完成：{result.OutputPath}");
        return true;
    }

    private static string[] GetEnabledScenePaths()
    {
        return EditorBuildSettings.scenes
            .Where(scene => scene.enabled && !string.IsNullOrEmpty(scene.path))
            .Select(scene => scene.path)
            .ToArray();
    }

    private string GetBuildOutputPath(BuildProfile profile)
    {
        var target = profile.BuildTarget;
        var outputName = GetBuildOutputName(profile);
        var version = profile.GetPlayerVersion(baseVersion);
        if (appendVersionToBuildName && !string.IsNullOrWhiteSpace(version))
        {
            outputName = $"{outputName}_{version}";
        }

        outputName = SanitizeFileName(outputName);

        var outputFolder = GetFullPath(buildOutputRootPath, profile.OutputFolderName, target.ToString());
        return target switch
        {
            BuildTarget.Android => Path.Combine(outputFolder, $"{outputName}.apk"),
            BuildTarget.StandaloneWindows => Path.Combine(outputFolder, $"{outputName}.exe"),
            BuildTarget.StandaloneWindows64 => Path.Combine(outputFolder, $"{outputName}.exe"),
            BuildTarget.StandaloneOSX => Path.Combine(outputFolder, $"{outputName}.app"),
            BuildTarget.WebGL => outputFolder,
            BuildTarget.iOS => outputFolder,
            _ => Path.Combine(outputFolder, outputName),
        };
    }

    private static string GetBuildOutputName(BuildProfile profile)
    {
        if (!string.IsNullOrWhiteSpace(profile.BuildOutputName))
        {
            return profile.BuildOutputName.Trim();
        }

        var buildProjectInfo = BuildProjectInfo.LoadAsset();
        return buildProjectInfo == null ? PlayerSettings.productName : buildProjectInfo.GetConfiguredProductName();
    }

    private static bool ApplyBuildProjectInfo()
    {
        var buildProjectInfo = BuildProjectInfo.LoadAsset();
        if (buildProjectInfo == null)
        {
            Debug.LogError($"没有找到项目基础信息配置：{BuildProjectInfo.AssetPath}");
            return false;
        }

        return buildProjectInfo.ApplyProjectInfo();
    }

    private static void EnsureBuildOutputFolder(string outputPath, BuildTarget target)
    {
        var folderPath = IsFolderBuildTarget(target) ? outputPath : Path.GetDirectoryName(outputPath);
        if (string.IsNullOrEmpty(folderPath))
        {
            Debug.LogError($"打包输出目录无效：{outputPath}");
            return;
        }

        Directory.CreateDirectory(folderPath);
    }

    private static bool IsFolderBuildTarget(BuildTarget target)
    {
        return target is BuildTarget.WebGL or BuildTarget.iOS;
    }

    private static BuildOptions GetBuildOptions(BuildProfile profile)
    {
        var options = BuildOptions.None;
        if (profile.DevelopmentBuild)
        {
            options |= BuildOptions.Development;
        }

        return options;
    }

    private static void LogBuildReport(BuildReport report, string outputPath)
    {
        var summary = report.summary;
        var message = $"打包结果：{summary.result}，平台：{summary.platform}，耗时：{summary.totalTime.TotalSeconds:F1}s，输出：{outputPath}";

        switch (summary.result)
        {
            case BuildResult.Succeeded:
                Debug.Log(message);
                break;
            case BuildResult.Cancelled:
                Debug.LogWarning(message);
                break;
            default:
                Debug.LogError(message);
                break;
        }
    }

    private static string SanitizeFileName(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return "Game";
        }

        foreach (var invalidChar in Path.GetInvalidFileNameChars())
        {
            fileName = fileName.Replace(invalidChar, '_');
        }

        return fileName;
    }

    private bool CopyPlatformModules(BuildProfile profile)
    {
        if (profile.ModuleFolders == null || profile.ModuleFolders.Count == 0)
        {
            return true;
        }

        foreach (var moduleFolder in profile.ModuleFolders)
        {
            if (moduleFolder == null || string.IsNullOrEmpty(moduleFolder.TargetFolder))
            {
                continue;
            }

            if (string.IsNullOrEmpty(moduleFolder.SourceFolder))
            {
                Debug.LogError($"渠道平台资源缺少外部存放子目录，工程接入子目录：{moduleFolder.TargetFolder}");
                return false;
            }

            var sourcePath = GetFullPath(platformModulesRootPath, moduleFolder.SourceFolder);
            var targetAssetPath = CombineAssetPath(platformImportRootPath, moduleFolder.TargetFolder);
            var targetFullPath = GetFullPath(targetAssetPath);

            if (!Directory.Exists(sourcePath))
            {
                Debug.LogError($"渠道平台资源外部存放目录不存在：{sourcePath}");
                return false;
            }

            EnsureAssetFolder(platformImportRootPath);
            DeleteAssetFolder(targetAssetPath);
            FileUtil.CopyFileOrDirectory(sourcePath, targetFullPath);
            Debug.Log($"已复制渠道平台资源：{sourcePath} -> {targetFullPath}");
        }

        return true;
    }

    private void RemoveUnselectedPlatformModules(BuildProfile currentProfile)
    {
        var selectedTargets = GetTargetFolders(currentProfile);
        foreach (var targetFolder in GetAllConfiguredTargetFolders())
        {
            if (selectedTargets.Contains(targetFolder))
            {
                continue;
            }

            var targetAssetPath = CombineAssetPath(platformImportRootPath, targetFolder);
            DeleteAssetFolder(targetAssetPath);
            Debug.Log($"已移除未选中的渠道平台资源：{targetAssetPath}");
        }
    }

    private static void AddDefineSymbols(BuildProfile profile)
    {
        if (profile.DefineSymbols == null)
        {
            return;
        }

        var targetGroup = profile.BuildTargetGroup;
        var defines = GetDefineSymbols(targetGroup);
        var isChanged = false;

        foreach (var defineSymbol in profile.DefineSymbols)
        {
            if (string.IsNullOrWhiteSpace(defineSymbol) || defines.Contains(defineSymbol))
            {
                continue;
            }

            defines.Add(defineSymbol);
            isChanged = true;
            Debug.Log($"已添加宏定义：{defineSymbol}");
        }

        if (isChanged)
        {
            SetDefineSymbols(targetGroup, defines);
        }
    }

    private static void RemoveUnselectedDefineSymbols(BuildProfile currentProfile)
    {
        var selectedDefines = new HashSet<string>();
        foreach (var defineSymbol in currentProfile.DefineSymbols ?? Array.Empty<string>())
        {
            if (!string.IsNullOrWhiteSpace(defineSymbol))
            {
                selectedDefines.Add(defineSymbol);
            }
        }

        var configuredDefines = GetAllConfiguredDefineSymbols();
        var targetGroup = currentProfile.BuildTargetGroup;
        var defines = GetDefineSymbols(targetGroup);
        var isChanged = false;

        foreach (var defineSymbol in configuredDefines)
        {
            if (selectedDefines.Contains(defineSymbol) || !defines.Remove(defineSymbol))
            {
                continue;
            }

            isChanged = true;
            Debug.Log($"已移除宏定义：{defineSymbol}");
        }

        if (isChanged)
        {
            SetDefineSymbols(targetGroup, defines);
        }
    }

    private static HashSet<string> GetTargetFolders(BuildProfile profile)
    {
        var targetFolders = new HashSet<string>();
        if (profile?.ModuleFolders == null)
        {
            return targetFolders;
        }

        foreach (var moduleFolder in profile.ModuleFolders)
        {
            if (moduleFolder != null && !string.IsNullOrEmpty(moduleFolder.TargetFolder))
            {
                targetFolders.Add(moduleFolder.TargetFolder);
            }
        }

        return targetFolders;
    }

    private static HashSet<string> GetAllConfiguredTargetFolders()
    {
        var targetFolders = new HashSet<string>();
        foreach (var profile in BuildProfileRegistry.Profiles)
        {
            foreach (var targetFolder in GetTargetFolders(profile))
            {
                targetFolders.Add(targetFolder);
            }
        }

        return targetFolders;
    }

    private static HashSet<string> GetAllConfiguredDefineSymbols()
    {
        var defineSymbols = new HashSet<string>();
        foreach (var profile in BuildProfileRegistry.Profiles)
        {
            if (profile.DefineSymbols == null)
            {
                continue;
            }

            foreach (var defineSymbol in profile.DefineSymbols)
            {
                if (!string.IsNullOrWhiteSpace(defineSymbol))
                {
                    defineSymbols.Add(defineSymbol);
                }
            }
        }

        return defineSymbols;
    }

    private static HashSet<string> GetDefineSymbols(BuildTargetGroup targetGroup)
    {
        var defineString = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
        var defines = new HashSet<string>();
        foreach (var item in defineString.Split(';'))
        {
            var define = item.Trim();
            if (!string.IsNullOrEmpty(define))
            {
                defines.Add(define);
            }
        }

        return defines;
    }

    private static void SetDefineSymbols(BuildTargetGroup targetGroup, HashSet<string> defines)
    {
        var defineList = new List<string>(defines);
        defineList.Sort(StringComparer.Ordinal);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, string.Join(";", defineList));
    }

    private static void EnsureAssetFolder(string assetFolderPath)
    {
        if (AssetDatabase.IsValidFolder(assetFolderPath))
        {
            return;
        }

        var folders = assetFolderPath.Split('/');
        var currentPath = folders[0];
        for (var i = 1; i < folders.Length; i++)
        {
            var nextPath = $"{currentPath}/{folders[i]}";
            if (!AssetDatabase.IsValidFolder(nextPath))
            {
                AssetDatabase.CreateFolder(currentPath, folders[i]);
            }

            currentPath = nextPath;
        }
    }

    private static void DeleteAssetFolder(string assetFolderPath)
    {
        if (!AssetDatabase.IsValidFolder(assetFolderPath))
        {
            return;
        }

        AssetDatabase.DeleteAsset(assetFolderPath);
    }

    private static string CombineAssetPath(string left, string right)
    {
        return $"{left.TrimEnd('/', '\\')}/{right.TrimStart('/', '\\')}";
    }

    private static string GetFullPath(params string[] pathParts)
    {
        var path = Path.Combine(pathParts);
        if (!Path.IsPathRooted(path))
        {
            path = Path.Combine(ProjectRootPath, path);
        }

        return Path.GetFullPath(path);
    }

    private static void OpenFolder(string folderPath)
    {
        try
        {
            Directory.CreateDirectory(folderPath);
            EditorUtility.OpenWithDefaultApp(folderPath);
        }
        catch (Exception exception)
        {
            Debug.LogError($"打开文件夹失败：{folderPath}\n{exception}");
        }
    }


    private static string ProjectRootPath
    {
        get
        {
            return Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        }
    }
}
