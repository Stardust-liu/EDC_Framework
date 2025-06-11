using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArchiveData;

public class AchievementManager : BaseIOCComponent<AchievementData>, ISendEvent
{
    /// <summary>
    /// 更新成就进度
    /// </summary>
    public void UpdateAchivementProgress<T>(string achievementID, int addCount = 1, float completeTime = -1f)where T: BaseAchievement{
        if(!IsUnlockAchievement(achievementID)){
            Data.UpdateAchivementProgress<T>(achievementID, addCount, completeTime);
            if(IsUnlockAchievement(achievementID)){
                UnlockAchievement(achievementID);
            }
        }
    }

    /// <summary>
    /// 是否解锁成就
    /// </summary>
    public bool IsUnlockAchievement(string achievement){
        return Data.IsUnlockAchievement(achievement);
    }

    /// <summary>
    /// 获取成就完成状态
    /// </summary>
    public AchievementProgressInfo GetAchievementProgress(string achievement){
        return Data.GetAchievementProgress(achievement);
    }

    /// <summary>
    /// 解锁成就
    /// </summary>
    private void UnlockAchievement(string achievement){
        this.SendEvent(new UnlockAchievement(achievement));
    }
}
