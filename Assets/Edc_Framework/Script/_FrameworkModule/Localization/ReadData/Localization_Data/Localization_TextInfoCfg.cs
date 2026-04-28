using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Localization_TextInfoCfg : BaseLocalizationInfoCfg<Localization_TextInfoCfg>
{
    private static Dictionary<string, string> localizationInfo = new();

    protected override void InitData()
    {
        base.InitData();
        localizationInfo ??= new();
    }

    protected override void SetData(string id)
    {
        localizationInfo.Add(id, GetString("Desc"));
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
    /// 获取本地化文字
    /// </summary>
    public string GetLocalizationText(string key)
    {
        return localizationInfo[key];
    }
}
