using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class HotUpdateLoader
{
    private const string HotUpdateAssemblyName = "HotUpdate_Game";
    private const string HotUpdateDllFileName = HotUpdateAssemblyName + ".dll.bytes";
    private const string HotUpdateEntryTypeName = "HotUpdateEntry";
    private const string HotUpdateInitMethodName = "Init";
    private const string HotUpdateReadyMethodName = "ReadyRegisteredModules";
    private const string HotUpdateEnterGameMethodName = "EnterGame";
    private const string HotUpdateQuitMethodName = "Quit";

    private static Assembly hotUpdateAssembly;
    private static bool hasLoadAttempted;

    public static async UniTask Load()
    {
        if (hotUpdateAssembly != null)
        {
            return;
        }

        hasLoadAttempted = true;
        hotUpdateAssembly = await LoadHotUpdateAssembly();
    }

    /// <summary>
    /// 加载Assembly
    /// </summary>
    private static async UniTask<Assembly> LoadHotUpdateAssembly()
    {
#if UNITY_EDITOR
        var assembly = GetLoadedHotUpdateAssembly();
        if (assembly != null)
        {
            return assembly;
        }
#endif

        var dllBytes = await LoadHotUpdateDllBytes();
        if (dllBytes == null || dllBytes.Length == 0)
        {
            LogManager.LogError($"HotUpdate dll not found: {HotUpdateDllFileName}");
            return null;
        }

        try
        {
            return Assembly.Load(dllBytes);
        }
        catch (Exception exception)
        {
            LogManager.LogError($"HotUpdate assembly load failed: {exception}");
            return null;
        }
    }


    /// <summary>
    /// 获取程序集
    /// </summary>
    private static Assembly GetLoadedHotUpdateAssembly()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(assembly => assembly.GetName().Name == HotUpdateAssemblyName);
    }

    /// <summary>
    /// 从StreamingAssets中读取热更新程序集文件
    /// </summary>
    private static async UniTask<byte[]> LoadHotUpdateDllBytes()
    {
        var path = Path.Combine(Application.streamingAssetsPath, HotUpdateDllFileName);
        var uri = path.Contains("://") ? path : "file://" + path;

        using var request = UnityWebRequest.Get(uri);
        await request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            return null;
        }

        return request.downloadHandler.data;
    }

    public static UniTask Init(GameStartInfo startInfo)
    {
        return InvokeHotUpdateMethod(HotUpdateInitMethodName, startInfo);
    }

    public static UniTask ReadyRegisteredModules()
    {
        return InvokeHotUpdateMethod(HotUpdateReadyMethodName);
    }

    public static UniTask EnterGame()
    {
        return InvokeHotUpdateMethod(HotUpdateEnterGameMethodName);
    }

    public static void Quit()
    {
        if (hotUpdateAssembly == null)
        {
            return;
        }

        InvokeHotUpdateMethod(HotUpdateQuitMethodName).Forget();
    }

    private static UniTask InvokeHotUpdateMethod(string methodName)
    {
        return InvokeHotUpdateMethod(methodName, Type.EmptyTypes, null);
    }

    private static UniTask InvokeHotUpdateMethod<T>(string methodName, T parameter)
    {
        return InvokeHotUpdateMethod(
            methodName,
            new[] { typeof(T) },
            new object[] { parameter });
    }

    private static async UniTask InvokeHotUpdateMethod(string methodName, Type[] parameterTypes, object[] parameters)
    {
        if (hotUpdateAssembly == null)
        {
            if (!hasLoadAttempted)
            {
                LogManager.LogError($"HotUpdate entry can not be called before loading assembly: {HotUpdateEntryTypeName}.{methodName}");
            }

            return;
        }

        var method = GetHotUpdateEntryMethod(methodName, parameterTypes);
        if (method == null)
        {
            LogManager.LogError($"HotUpdate entry not found: {HotUpdateEntryTypeName}.{methodName}");
            return;
        }

        object result;
        try
        {
            result = method.Invoke(null, parameters);
        }
        catch (TargetInvocationException exception)
        {
            LogManager.LogError($"HotUpdate entry failed: {HotUpdateEntryTypeName}.{methodName}\n{exception.InnerException ?? exception}");
            return;
        }
        catch (Exception exception)
        {
            LogManager.LogError($"HotUpdate entry failed: {HotUpdateEntryTypeName}.{methodName}\n{exception}");
            return;
        }

        if (result is UniTask task)
        {
            await task;
        }
    }

    private static MethodInfo GetHotUpdateEntryMethod(string methodName, Type[] parameterTypes)
    {
        var entryType = hotUpdateAssembly?.GetType(HotUpdateEntryTypeName);
        return entryType?.GetMethod(
            methodName,
            BindingFlags.Public | BindingFlags.Static,
            null,
            parameterTypes,
            null);
    }
}
