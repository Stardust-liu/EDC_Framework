using System;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// 平台成就管理器，负责发现并启动当前平台编译进来的成就服务。
/// </summary>
public class PlatformAchievementManager : BaseIOCComponent, IGameQuit
{
    private const string PlatformAssemblyName = "EdcFramework.Platform";
    private const string PlatformServiceNamespace = "EdcFramework.Platform";
    private BasePlatformAchievementService achievementService;

    protected override void Init()
    {
        base.Init();
        RegisterActivePlatformService();
        achievementService?.Init();
    }

    protected override void Uninstall()
    {
        UninstallAchievementService();
        base.Uninstall();
    }

    public void OnGameQuit()
    {
        UninstallAchievementService();
    }

    private void RegisterActivePlatformService()
    {
        var serviceTypes = new List<Type>(GetPlatformAchievementServiceTypes());
        if (serviceTypes.Count == 0)
        {
            return;
        }

        if (serviceTypes.Count > 1)
        {
            var serviceNames = new List<string>();
            foreach (var serviceType in serviceTypes)
            {
                serviceNames.Add(serviceType.Name);
            }

            LogManager.LogError($"检测到多个平台成就服务：{string.Join("、", serviceNames)}，请检查当前构建平台配置");
            return;
        }

        var activeServiceType = serviceTypes[0];
        try
        {
            if (Activator.CreateInstance(activeServiceType) is BasePlatformAchievementService service)
            {
                RegisterService(service);
            }
            else
            {
                LogManager.LogError($"创建平台成就服务失败：{activeServiceType.Name} 不是有效的平台成就服务");
            }
        }
        catch (Exception exception)
        {
            LogManager.LogError($"创建平台成就服务失败：{activeServiceType.Name}\n{exception}");
        }
    }

    private void RegisterService(BasePlatformAchievementService service)
    {
        if (service == null)
        {
            return;
        }

        var serviceType = service.GetType();
        if (achievementService != null)
        {
            LogManager.LogError($"平台成就服务重复注册：{serviceType.Name}");
            return;
        }

        achievementService = service;
    }

    private void UninstallAchievementService()
    {
        achievementService?.Uninstall();
        achievementService = null;
    }

    private static IEnumerable<Type> GetPlatformAchievementServiceTypes()
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (assembly.GetName().Name != PlatformAssemblyName)
            {
                continue;
            }

            Type[] types;
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException exception)
            {
                types = exception.Types;
            }

            foreach (var type in types)
            {
                if (type == null || type.IsAbstract || type.Namespace != PlatformServiceNamespace || !typeof(BasePlatformAchievementService).IsAssignableFrom(type))
                {
                    continue;
                }

                if (type.GetConstructor(Type.EmptyTypes) == null)
                {
                    continue;
                }

                yield return type;
            }
        }
    }
}
