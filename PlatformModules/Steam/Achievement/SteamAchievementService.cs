#if EDC_STEAM
using System.Collections.Generic;
using Steamworks;

namespace EdcFramework.Platform
{
/// <summary>
/// Steam 成就服务，负责把框架成就事件同步到 Steam。
/// </summary>
public class SteamAchievementService : BasePlatformAchievementService
{
    private readonly Dictionary<string, PlatformAchievementBinding> achievementBindings = new();
    private readonly List<string> pendingUnlockAchievements = new();
    private readonly List<PendingProgressInfo> pendingProgressInfos = new();

    private Callback<UserStatsReceived_t> userStatsReceivedCallback;
    private Callback<UserStatsStored_t> userStatsStoredCallback;
    private Callback<UserAchievementStored_t> userAchievementStoredCallback;
    private bool isStatsReady;

    protected override bool InitializePlatform()
    {
        if (!SteamPlatformRuntime.Retain())
        {
            return false;
        }

        RegisterAchievementBindings();
        userStatsReceivedCallback = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
        userStatsStoredCallback = Callback<UserStatsStored_t>.Create(OnUserStatsStored);
        userAchievementStoredCallback = Callback<UserAchievementStored_t>.Create(OnUserAchievementStored);
        isStatsReady = true;

        return true;
    }

    protected override void UnlockPlatformAchievement(string achievementID)
    {
        if (!isStatsReady)
        {
            pendingUnlockAchievements.Add(achievementID);
            return;
        }

        UnlockSteamAchievement(achievementID);
    }

    protected override void UpdatePlatformAchievementProgress(string achievementID, int progress)
    {
        if (!isStatsReady)
        {
            pendingProgressInfos.Add(new PendingProgressInfo(achievementID, progress));
            return;
        }

        UpdateSteamAchievementProgress(achievementID, progress);
    }

    protected override void UninstallPlatform()
    {
        userStatsReceivedCallback?.Dispose();
        userStatsStoredCallback?.Dispose();
        userAchievementStoredCallback?.Dispose();
        userStatsReceivedCallback = null;
        userStatsStoredCallback = null;
        userAchievementStoredCallback = null;
        pendingUnlockAchievements.Clear();
        pendingProgressInfos.Clear();
        achievementBindings.Clear();
        isStatsReady = false;
        SteamPlatformRuntime.Release();
    }

    /// <summary>
    /// 登记 Steam 成就映射。没有登记的成就会默认使用本地成就 ID 作为 Steam Achievement API Name。
    /// </summary>
    protected virtual void RegisterAchievementBindings()
    {
    }

    protected void RegisterAchievement(string achievementID, string steamAchievementID = null, string steamStatID = null, int targetProgress = 0, bool isShowProgress = true)
    {
        if (string.IsNullOrEmpty(achievementID))
        {
            return;
        }

        achievementBindings[achievementID] = new PlatformAchievementBinding
        {
            achievementID = achievementID,
            platformAchievementID = steamAchievementID,
            platformProgressID = steamStatID,
            targetProgress = targetProgress,
            isShowProgress = isShowProgress,
        };
    }

    private void UnlockSteamAchievement(string achievementID)
    {
        var binding = GetAchievementBinding(achievementID);
        var steamAchievementID = binding.GetPlatformAchievementID();
        if (string.IsNullOrEmpty(steamAchievementID))
        {
            return;
        }

        if (SteamUserStats.GetAchievement(steamAchievementID, out var isAchieved) && isAchieved)
        {
            return;
        }

        if (!SteamUserStats.SetAchievement(steamAchievementID))
        {
            LogManager.LogWarning($"Steam 成就解锁失败：{steamAchievementID}");
            return;
        }

        StoreSteamStats();
    }

    private void UpdateSteamAchievementProgress(string achievementID, int progress)
    {
        var binding = GetAchievementBinding(achievementID);
        if (binding.targetProgress <= 0)
        {
            return;
        }

        var currentProgress = ClampProgress(progress, binding.targetProgress);
        var steamStatID = binding.GetPlatformProgressID();
        if (!string.IsNullOrEmpty(steamStatID) && !SteamUserStats.SetStat(steamStatID, currentProgress))
        {
            LogManager.LogWarning($"Steam 成就进度写入失败：{steamStatID}");
            return;
        }

        var steamAchievementID = binding.GetPlatformAchievementID();
        if (binding.isShowProgress && !string.IsNullOrEmpty(steamAchievementID))
        {
            SteamUserStats.IndicateAchievementProgress(steamAchievementID, (uint)currentProgress, (uint)binding.targetProgress);
        }

        StoreSteamStats();
    }

    private PlatformAchievementBinding GetAchievementBinding(string achievementID)
    {
        if (achievementBindings.TryGetValue(achievementID, out var binding))
        {
            return binding;
        }

        return new PlatformAchievementBinding
        {
            achievementID = achievementID,
        };
    }

    private void FlushPendingAchievementEvents()
    {
        foreach (var pendingProgressInfo in pendingProgressInfos)
        {
            UpdateSteamAchievementProgress(pendingProgressInfo.achievementID, pendingProgressInfo.progress);
        }

        foreach (var achievementID in pendingUnlockAchievements)
        {
            UnlockSteamAchievement(achievementID);
        }

        pendingProgressInfos.Clear();
        pendingUnlockAchievements.Clear();
    }

    private void StoreSteamStats()
    {
        if (!SteamUserStats.StoreStats())
        {
            LogManager.LogWarning("Steam 成就与统计数据提交失败");
        }
    }

    private void OnUserStatsReceived(UserStatsReceived_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            LogManager.LogWarning($"Steam 成就与统计数据接收失败：{callback.m_eResult}");
            return;
        }

        isStatsReady = true;
        FlushPendingAchievementEvents();
    }

    private void OnUserStatsStored(UserStatsStored_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            LogManager.LogWarning($"Steam 成就与统计数据提交回调失败：{callback.m_eResult}");
        }
    }

    private void OnUserAchievementStored(UserAchievementStored_t callback)
    {
        LogManager.Log($"Steam 成就已更新：{callback.m_rgchAchievementName}");
    }

    private static int ClampProgress(int progress, int targetProgress)
    {
        if (progress < 0)
        {
            return 0;
        }

        return progress > targetProgress ? targetProgress : progress;
    }

    private readonly struct PendingProgressInfo
    {
        public readonly string achievementID;
        public readonly int progress;

        public PendingProgressInfo(string achievementID, int progress)
        {
            this.achievementID = achievementID;
            this.progress = progress;
        }
    }
}
}
#endif
