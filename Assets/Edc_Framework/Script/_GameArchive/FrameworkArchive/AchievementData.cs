using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArchiveData{
    public class AchievementData : BaseGameArchive<AchievementData>
    {
        public Dictionary<string, BaseAchievement> unlockAchievement = new();

        /// <summary>
        /// 更新成就进度
        /// </summary>
        public void UpdateAchivementProgress<T>(string achievementID, int addCount, float completeTime = -1)where  T: BaseAchievement{
            if(!unlockAchievement.ContainsKey(achievementID)){
                unlockAchievement.Add(achievementID, Activator.CreateInstance<T>());
                unlockAchievement[achievementID].Init(achievementID);
                unlockAchievement[achievementID].UpdateAchivementProgress(addCount, completeTime);
            }
            else{
                unlockAchievement[achievementID].UpdateAchivementProgress(addCount, completeTime);
            }
            SaveDataNow();
        }

        /// <summary>
        /// 获取成就完成状态
        /// </summary>
        public AchievementProgressInfo GetAchievementProgress(string achievement){
            if(unlockAchievement.ContainsKey(achievement)){
                return unlockAchievement[achievement].progressInfo;
            }
            else{
                return null;
            }
        }

        /// <summary>
        /// 是否解锁成就
        /// </summary>
        public bool IsUnlockAchievement(string achievement){
            if(unlockAchievement.ContainsKey(achievement)){
                return unlockAchievement[achievement].isUnlockAchievement;
            }
            else{
                return false;
            }
        }
    }
}
