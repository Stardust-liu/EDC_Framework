using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public sealed class BuildModuleFolder
{
    public BuildModuleFolder(string sourceFolder, string targetFolder)
    {
        SourceFolder = sourceFolder;
        TargetFolder = targetFolder;
    }

    /// <summary>
    /// 渠道平台资源在外部存放目录下的子目录
    /// </summary>
    public string SourceFolder { get; }

    /// <summary>
    /// 渠道平台资源复制到工程接入目录后的子目录
    /// </summary>
    public string TargetFolder { get; }
}

public readonly struct BuildProfileDisplayItem
{
    public BuildProfileDisplayItem(string name, string value)
    {
        Name = name;
        Value = value;
    }

    /// <summary>
    /// 显示在打包设置面板中的字段名
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// 显示在打包设置面板中的字段值
    /// </summary>
    public string Value { get; }
}

public readonly struct BuildProfileDisplayContext
{
    public BuildProfileDisplayContext(string baseVersion, string playerVersion, string buildOutputName)
    {
        BaseVersion = baseVersion;
        PlayerVersion = playerVersion;
        BuildOutputName = buildOutputName;
    }

    /// <summary>
    /// 打包设置面板中填写的基础版本号
    /// </summary>
    public string BaseVersion { get; }

    /// <summary>
    /// 当前配置最终写入 PlayerSettings 的版本号
    /// </summary>
    public string PlayerVersion { get; }

    /// <summary>
    /// 当前配置最终使用的打包产物名称
    /// </summary>
    public string BuildOutputName { get; }
}

public abstract class BuildProfile
{
    private static readonly IReadOnlyList<BuildModuleFolder> EmptyModuleFolders = Array.Empty<BuildModuleFolder>();
    private static readonly IReadOnlyList<string> EmptyDefineSymbols = Array.Empty<string>();

    /// <summary>
    /// 配置唯一ID，用来保存和查找当前选择的打包配置
    /// </summary>
    public abstract string ProfileId { get; }

    /// <summary>
    /// 配置说明，显示在打包配置下拉框和当前构建设置中
    /// </summary>
    public abstract string DisplayName { get; }

    /// <summary>
    /// 配置排序，数值越小越靠前
    /// </summary>
    public virtual int SortOrder => 0;

    /// <summary>
    /// 构建平台组，例如 Standalone、Android、iOS
    /// </summary>
    public virtual BuildTargetGroup BuildTargetGroup => BuildTargetGroup.Standalone;

    /// <summary>
    /// 具体构建平台，例如 StandaloneWindows64、Android、iOS
    /// </summary>
    public virtual BuildTarget BuildTarget => BuildTarget.StandaloneWindows64;

    /// <summary>
    /// 当前配置需要接入到工程中的渠道平台资源
    /// </summary>
    public virtual IReadOnlyList<BuildModuleFolder> ModuleFolders => EmptyModuleFolders;

    /// <summary>
    /// 当前配置需要启用的宏定义
    /// </summary>
    public virtual IReadOnlyList<string> DefineSymbols => EmptyDefineSymbols;

    /// <summary>
    /// 是否启用 Unity 的 Development Build
    /// </summary>
    public virtual bool DevelopmentBuild => false;

    /// <summary>
    /// 版本后缀，保留给需要区分 dev、demo 等版本标记的配置使用
    /// </summary>
    public virtual string VersionSuffix => string.Empty;

    /// <summary>
    /// 打包产物的基础名称。为空时使用 PlayerSettings.productName
    /// </summary>
    public virtual string BuildOutputName => string.Empty;

    /// <summary>
    /// 当前配置的打包输出子目录名
    /// </summary>
    public virtual string OutputFolderName => ProfileId;

    /// <summary>
    /// 当前配置额外展示在打包设置面板中的信息
    /// </summary>
    public virtual IEnumerable<BuildProfileDisplayItem> GetDisplayItems(BuildProfileDisplayContext context)
    {
        yield return new BuildProfileDisplayItem("配置说明", DisplayName);
        yield return new BuildProfileDisplayItem("构建平台", $"{BuildTargetGroup} / {BuildTarget}");
        yield return new BuildProfileDisplayItem("宏定义预览", JoinList(DefineSymbols));
        yield return new BuildProfileDisplayItem("平台资源预览", JoinModuleFolders(ModuleFolders));
        yield return new BuildProfileDisplayItem("打包输出名", context.BuildOutputName);
        yield return new BuildProfileDisplayItem("Development Build", DevelopmentBuild ? "开启" : "关闭");
    }

    /// <summary>
    /// 应用当前配置特有的 PlayerSettings 或其它编辑器设置
    /// </summary>
    public virtual bool ApplyProfileSettings()
    {
        return true;
    }

    /// <summary>
    /// 根据基础版本号获取最终写入 PlayerSettings.bundleVersion 的版本号
    /// </summary>
    public string GetPlayerVersion(string baseVersion)
    {
        return string.IsNullOrWhiteSpace(baseVersion) ? "0.1" : baseVersion.Trim();
    }

    protected static string JoinList(IReadOnlyList<string> values)
    {
        if (values == null || values.Count == 0)
        {
            return "无";
        }

        var result = new List<string>();
        foreach (var value in values)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                result.Add(value);
            }
        }

        return result.Count == 0 ? "无" : string.Join("，", result);
    }

    protected static string JoinModuleFolders(IReadOnlyList<BuildModuleFolder> moduleFolders)
    {
        if (moduleFolders == null || moduleFolders.Count == 0)
        {
            return "无";
        }

        var result = new List<string>();
        foreach (var moduleFolder in moduleFolders)
        {
            if (moduleFolder != null)
            {
                result.Add($"{moduleFolder.SourceFolder} -> {moduleFolder.TargetFolder}");
            }
        }

        return result.Count == 0 ? "无" : string.Join("，", result);
    }
}

public static class BuildProfileRegistry
{
    private static List<BuildProfile> profiles;

    public static IReadOnlyList<BuildProfile> Profiles
    {
        get
        {
            profiles ??= LoadProfiles();
            return profiles;
        }
    }

    public static void Refresh()
    {
        profiles = LoadProfiles();
    }

    public static BuildProfile GetDefaultProfile()
    {
        return Profiles.Count > 0 ? Profiles[0] : null;
    }

    public static bool TryGetProfile(string profileId, out BuildProfile profile)
    {
        foreach (var item in Profiles)
        {
            if (string.Equals(item.ProfileId, profileId, StringComparison.OrdinalIgnoreCase))
            {
                profile = item;
                return true;
            }
        }

        profile = null;
        return false;
    }

    private static List<BuildProfile> LoadProfiles()
    {
        var result = new List<BuildProfile>();
        var profileIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var type in TypeCache.GetTypesDerivedFrom<BuildProfile>())
        {
            if (type.IsAbstract || type.GetConstructor(Type.EmptyTypes) == null)
            {
                continue;
            }

            try
            {
                if (Activator.CreateInstance(type) is BuildProfile profile)
                {
                    if (string.IsNullOrWhiteSpace(profile.ProfileId))
                    {
                        Debug.LogError($"打包配置 {type.Name} 的 ProfileId 不能为空");
                        continue;
                    }

                    if (!profileIds.Add(profile.ProfileId))
                    {
                        Debug.LogError($"打包配置 ProfileId 重复：{profile.ProfileId}，已忽略 {type.Name}");
                        continue;
                    }

                    result.Add(profile);
                }
            }
            catch (Exception exception)
            {
                Debug.LogError($"加载打包配置失败：{type.Name}\n{exception}");
            }
        }

        result.Sort((left, right) =>
        {
            var orderCompare = left.SortOrder.CompareTo(right.SortOrder);
            return orderCompare != 0
                ? orderCompare
                : string.Compare(left.ProfileId, right.ProfileId, StringComparison.Ordinal);
        });

        return result;
    }
}
