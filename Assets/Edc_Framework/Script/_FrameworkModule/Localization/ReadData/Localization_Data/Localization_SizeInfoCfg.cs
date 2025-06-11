using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localization_SizeInfoCfg : BaseLocalizationInfoCfg<Localization_SizeInfoCfg>
{
    private static Dictionary<string, Vector2> localizationInfo = new();

    protected override void SetData()
    {
        if (isAddInfo)
        {
            var pos = new Vector2(GetFloat("X"), GetFloat("Y"));
            localizationInfo.Add(GetString("ID"), pos);
        }
        else
        {
            localizationInfo.Remove(GetString("ID"));
        }
    }

    /// <summary>
    /// 获取本地化尺寸
    /// </summary>
    public Vector2 GetLocalizationSize(string key)
    {
        return localizationInfo[key];
    }
    
    public override void CleanLocalizationData()
    {
        localizationInfo.Clear();
    }
}
