using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class AchievementValueInfoVO{
    public int targetCount;
    public float targetTimeLimit;
    public bool isHideAchievement;
}


public class BaseAchievementInfoCfg<T> : ParsCsv<T> where T : class, new()
{
    protected static Dictionary<string, AchievementValueInfoVO> valueInfo;

    protected override void InitData()
    {
        if (valueInfo == null)
        {
            valueInfo = new(RowCount);
        }
    }

    protected override void SetData()
    {
        var key = GetString("ID");
        var achievementValueInfoVO = new AchievementValueInfoVO();
        achievementValueInfoVO.targetCount = GetInt("Count");
        achievementValueInfoVO.targetTimeLimit = GetFloat("TimeLimit");
        achievementValueInfoVO.isHideAchievement = GetBool("IsHide");
        valueInfo.Add(key, achievementValueInfoVO);
    }

    /// <summary>
    /// 获取数值信息
    /// </summary>
    public AchievementValueInfoVO GetValueInfo(string id){
        return valueInfo[id];
    }
}
