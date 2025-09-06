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
        if (localizationInfo == null)
        {
            localizationInfo = new(RowCount);
        }
    }

    protected override void SetData()
    {
        if (isAddInfo)
        {
            localizationInfo.Add(GetString("ID"), GetString("Desc"));
        }
        else
        {
            localizationInfo.Remove(GetString("ID"));
        }
    }

    /// <summary>
    /// 获取本地化文字
    /// </summary>
    public string GetLocalizationText(string key)
    {
        return localizationInfo[key];
    }
    
    public override void CleanLocalizationData()
    {
        localizationInfo?.Clear();
    }
}
