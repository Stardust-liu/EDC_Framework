using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Localization_AssetInfoCfg : BaseLocalizationInfoCfg<Localization_AssetInfoCfg>
{
    private static Dictionary<string, string> localizationInfo;
    private static List<string> addressableInfo;
    private static AssetManager assetManager;
    
    protected override void InitData()
    {
        base.InitData();
        if(localizationInfo == null)
        {
            localizationInfo = new();
            addressableInfo = new();
            AssetManager.Init(out assetManager);
        }
        addressableInfo.Clear();
    }

    protected override void SetData(string id)
    {
        var resourcePath = GetString("AssetsPath");
        localizationInfo.Add(id, resourcePath);
        addressableInfo.Add(resourcePath);
    }

    protected override void RemoveLocalizationData(string key)
    {
        var resourcePath = localizationInfo[key];
        assetManager.Release(resourcePath);
        localizationInfo.Remove(key);
    }

    public override void CleanLocalizationData()
    {
        base.CleanLocalizationData();
        localizationInfo?.Clear();
        assetManager?.ReleaseAll();
    }

    /// <summary>
    /// 加载资源信息
    /// </summary>
    public async UniTask LoadInfo()
    {
        await assetManager.AddLoad(addressableInfo);
        addressableInfo.Clear();
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
}
