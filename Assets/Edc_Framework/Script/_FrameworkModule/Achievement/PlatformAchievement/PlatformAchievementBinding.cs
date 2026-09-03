using System;

/// <summary>
/// 本地成就与平台成就之间的映射信息。
/// </summary>
[Serializable]
public class PlatformAchievementBinding
{
    public string achievementID;
    public string platformAchievementID;
    public string platformProgressID;
    public int targetProgress;
    public bool isShowProgress = true;

    public string GetPlatformAchievementID()
    {
        return string.IsNullOrEmpty(platformAchievementID) ? achievementID : platformAchievementID;
    }

    public string GetPlatformProgressID()
    {
        return string.IsNullOrEmpty(platformProgressID) ? achievementID : platformProgressID;
    }
}
