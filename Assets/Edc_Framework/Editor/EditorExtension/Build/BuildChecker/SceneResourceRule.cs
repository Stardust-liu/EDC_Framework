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

