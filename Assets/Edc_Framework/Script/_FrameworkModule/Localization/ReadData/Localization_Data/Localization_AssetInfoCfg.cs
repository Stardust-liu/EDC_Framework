using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localization_AssetInfoCfg : BaseLocalizationInfoCfg<Localization_AssetInfoCfg>
{
    private static Dictionary<string, ResourcePath> localizationInfo = new();

    protected override void SetData()
    {
        if (isAddInfo)
        {
            var resourcePath = new ResourcePath(GetString("AB_FileNmae"), GetString("AssetsPath"));
            localizationInfo.Add(GetString("ID"), resourcePath);
        }
        else
        {
            localizationInfo.Remove(GetString("ID"));
        }
    }

    /// <summary>
    /// 获取本地化资源
    /// </summary>
    public T GetLocalizationAsset<T>(string key, string abName) where T : Object
    {
        return Hub.Resources.GetAssets<T>(abName, localizationInfo[key]);
    }

    public override void CleanLocalizationData()
    {
        localizationInfo.Clear();
    }
}
