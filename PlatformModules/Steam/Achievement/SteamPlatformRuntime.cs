#if EDC_STEAM
using System;
using Steamworks;

namespace EdcFramework.Platform
{
/// <summary>
/// Steam 平台运行时，统一负责 SteamAPI 初始化、回调驱动与关闭。
/// </summary>
public static class SteamPlatformRuntime
{
    private static int retainCount;
    private static bool isInitialized;
    private static SteamCallbackRunner callbackRunner;

    public static bool IsInitialized { get { return isInitialized; } }

    public static bool Retain()
    {
        if (isInitialized)
        {
            retainCount++;
            return true;
        }

        try
        {
            var initResult = SteamAPI.InitEx(out var errorMessage);
            if (initResult != ESteamAPIInitResult.k_ESteamAPIInitResult_OK)
            {
                LogManager.LogWarning($"SteamAPI 初始化失败：{initResult} {errorMessage}");
                return false;
            }

            isInitialized = true;
            retainCount = 1;
            callbackRunner = new SteamCallbackRunner();
            Hub.Update?.AddUpdate(callbackRunner);
            return true;
        }
        catch (Exception exception)
        {
            LogManager.LogError($"SteamAPI 初始化异常：{exception}");
            return false;
        }
    }

    public static void Release()
    {
        if (!isInitialized)
        {
            return;
        }

        retainCount = Math.Max(0, retainCount - 1);
        if (retainCount > 0)
        {
            return;
        }

        if (callbackRunner != null)
        {
            Hub.Update?.RemoveUpdate(callbackRunner);
            callbackRunner = null;
        }

        SteamAPI.Shutdown();
        isInitialized = false;
    }

    private class SteamCallbackRunner : IUpdate
    {
        public void OnUpdate()
        {
            if (isInitialized)
            {
                SteamAPI.RunCallbacks();
            }
        }
    }
}
}
#endif
