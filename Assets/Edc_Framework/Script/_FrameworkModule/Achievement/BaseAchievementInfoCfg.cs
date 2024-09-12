using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class AchievementValueInfoVO{
    public int targetCount;
    public float targetTimeLimit;
    public bool isHideAchievement;
}

public class AchievementDescInfoVO{
    public string achievementName;
    public string achievementDesc;
}

public class BaseAchievementInfoCfg<T> : ParsCsv<T> where T : class, new()
{
    protected static Dictionary<string, AchievementValueInfoVO> valueInfo = new ();
    protected static Dictionary<string, AchievementDescInfoVO> descInfo = new ();
    protected bool isInfoCfgState;//路径需要设置为先读取数值信息，然后修改isInfoCfgState为true，再读取描述信息
    public ReadOnlyDictionary<string, AchievementValueInfoVO> ValueInfo = new(valueInfo);
    public ReadOnlyDictionary<string, AchievementDescInfoVO> DescInfo = new(descInfo);


    protected override void SetData()
    {
        var key = GetString("ID");
        if(!isInfoCfgState){
            var achievementValueInfoVO = new AchievementValueInfoVO();
            achievementValueInfoVO.targetCount = GetInt("Count");
            achievementValueInfoVO.targetTimeLimit = GetFloat("TimeLimit");
            achievementValueInfoVO.isHideAchievement = GetBool("IsHide");
            valueInfo.Add(key, achievementValueInfoVO);
        }
        else{
            var achievementDescInfoVO = new AchievementDescInfoVO();
            achievementDescInfoVO.achievementName = GetString("Name");
            achievementDescInfoVO.achievementDesc = GetString("Desc");
            descInfo.Add(key, achievementDescInfoVO);
        }
    }

    /// <summary>
    /// 获取数值信息
    /// </summary>
    public AchievementValueInfoVO GetValueInfo(string id){
        return valueInfo[id];
    }

    /// <summary>
    /// 获取文字描述信息
    /// </summary>
    public AchievementDescInfoVO GetDescInfo(string id){
        return descInfo[id];
    }
}
