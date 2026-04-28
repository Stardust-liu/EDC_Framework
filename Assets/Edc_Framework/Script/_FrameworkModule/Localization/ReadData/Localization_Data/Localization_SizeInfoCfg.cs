using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localization_SizeInfoCfg : BaseLocalizationInfoCfg<Localization_SizeInfoCfg>
{
    private static Dictionary<string, Vector2> localizationInfo;

    protected override void InitData()
    {
        base.InitData();
        localizationInfo ??= new();
    }

    protected override void SetData(string id)
    {
        var pos = new Vector2(GetFloat("X"), GetFloat("Y"));
        localizationInfo.Add(id, pos);
    }

    protected override void RemoveLocalizationData(string key)
    {
        localizationInfo.Remove(key);
    }

    public override void CleanLocalizationData()
    {
        base.CleanLocalizationData();
        localizationInfo?.Clear();
    }

    /// <summary>
    /// 获取本地化尺寸
    /// </summary>
    public Vector2 GetLocalizationSize(string key)
    {
        return localizationInfo[key];
    }
}
