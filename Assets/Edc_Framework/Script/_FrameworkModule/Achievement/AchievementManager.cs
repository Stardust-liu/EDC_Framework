using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArchiveData;

public class AchievementEventName{
    public const string unlockAchievement = nameof(unlockAchievement);
}

public class AchievementManager
{
    public static readonly EventCenter eventCenter = new EventCenter();
    private readonly AchievementData achievementData;

    public AchievementManager(){
        achievementData = GameArchive.AchievementData;
    }

    /// <summary>
    /// 更新成就进度
    /// </summary>
    public void UpdateAchivementProgress<T>(string achievementID, int addCount = 1, float completeTime = -1f)where T: BaseAchievement{
        if(!IsUnlockAchievement(achievementID)){
            achievementData.UpdateAchivementProgress<T>(achievementID, addCount, completeTime);
            if(IsUnlockAchievement(achievementID)){
                UnlockAchievement(achievementID);
            }
        }
    }

    /// <summary>
    /// 是否解锁成就
    /// </summary>
    public bool IsUnlockAchievement(string achievement){
        return achievementData.IsUnlockAchievement(achievement);
    }

    /// <summary>
    /// 获取成就完成状态
    /// </summary>
    public AchievementProgressInfo GetAchievementProgress(string achievement){
        return achievementData.GetAchievementProgress(achievement);
    }

    /// <summary>
    /// 解锁成就
    /// </summary>
    private void UnlockAchievement(string achievement){
        eventCenter.OnEvent(AchievementEventName.unlockAchievement, achievement);
    }
}