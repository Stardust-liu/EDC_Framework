using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Localization_AssetInfoCfg : BaseLocalizationInfoCfg<Localization_AssetInfoCfg>
{
    private static Dictionary<string, string> localizationInfo;
    private static List<string> addressableInfo;
    private static IResourceOwner resourceOwner;
    
    protected override void InitData()
    {
        base.InitData();
        if(localizationInfo == null)
        {
            localizationInfo = new();
            addressableInfo = new();
        }
        if (resourceOwner == null)
        {
            resourceOwner = Hub.Resources.CreateOwner(nameof(Localization_AssetInfoCfg));
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
        resourceOwner?.ReleaseAsset(resourcePath);
        localizationInfo.Remove(key);
    }

    public override void CleanLocalizationData()
    {
        base.CleanLocalizationData();
        localizationInfo?.Clear();
        addressableInfo?.Clear();
        resourceOwner?.ReleaseAll();
        resourceOwner = null;
    }

    /// <summary>
    /// 加载资源信息
    /// </summary>
    public async UniTask LoadInfo()
    {
        if (addressableInfo == null || addressableInfo.Count == 0)
        {
            return;
        }

        var tasks = new UniTask[addressableInfo.Count];
        for (var i = 0; i < addressableInfo.Count; i++)
        {
            tasks[i] = resourceOwner.LoadAsset(addressableInfo[i]);
        }

        await UniTask.WhenAll(tasks);
        addressableInfo.Clear();
    }

    /// <summary>
    /// 获取本地化资源
    /// </summary>
    public T GetLocalizationAsset<T>(string key) where T : Object
    {
        var resourcePath = localizationInfo[key];
        if (typeof(T) == typeof(Sprite))
        {
            var tex = resourceOwner.GetAsset<Texture2D>(resourcePath);
            if (tex == null)
            {
                return null;
            }
            var spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);
            return spr as T;
        }
        else
        {
            return resourceOwner.GetAsset<T>(resourcePath);
        }
    }
}
