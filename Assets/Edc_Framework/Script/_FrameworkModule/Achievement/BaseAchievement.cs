using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BaseAchievement
{
    public AchievementProgressInfo progressInfo;
    public float targetTimeLimit;
    public bool isUnlockAchievement;
    public virtual void Init(string achievementID){
        progressInfo = new AchievementProgressInfo();
    }

    public void UpdateAchivementProgress(int addCount, float completeTime){
        if(completeTime <= targetTimeLimit){
            progressInfo.UpdateAchivementProgress(addCount);
            isUnlockAchievement = progressInfo.IsUnlockAchievement();
        }
    }
}

public class AchievementProgressInfo{
    public int completeCount;
    public int targetCount;
    public float progress;

    public void UpdateAchivementProgress(int addCount){
        completeCount = Mathf.Min(completeCount + addCount, targetCount);
    }

    public bool IsUnlockAchievement(){
        return completeCount == targetCount;
    }
}
