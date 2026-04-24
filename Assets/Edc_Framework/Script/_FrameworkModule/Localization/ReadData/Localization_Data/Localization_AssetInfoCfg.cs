using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localization_AssetInfoCfg : BaseLocalizationInfoCfg<Localization_AssetInfoCfg>
{
    private static Dictionary<string, string> localizationInfo;


    protected override void InitData()
    {
        localizationInfo = new(RowCount);
    }

    protected override void SetData()
    {
        if (isAddInfo)
        {
            var resourcePath = GetString("AssetsPath");
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
    public T GetLocalizationAsset<T>(string key) where T : Object
    {
        if (typeof(T) == typeof(Sprite))
        {
            var tex = Hub.Resources.Get<Texture2D>(localizationInfo[key]);
            var spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);
            return spr as T;
        }
        else
        {
            return Hub.Resources.Get<T>(localizationInfo[key]);
        }
    }

    public override void CleanLocalizationData()
    {
        localizationInfo?.Clear();
    }
}
