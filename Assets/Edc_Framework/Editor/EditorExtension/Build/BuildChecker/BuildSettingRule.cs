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
