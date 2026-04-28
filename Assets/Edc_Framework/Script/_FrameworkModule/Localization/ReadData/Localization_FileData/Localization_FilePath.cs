using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Localization_FileDataVO
{
    public string resourcePath;
    public LocalizationType localizationType;

    public Localization_FileDataVO(string _resourcePath, LocalizationType _localizationType)
    {
        resourcePath = _resourcePath;
        localizationType = _localizationType;
    }
}

public class Localization_FileData : ParsCsv<Localization_FileData>
{
    private Dictionary<string, Dictionary<SystemLanguage, List<Localization_FileDataVO>>> localizationInfo;
    public AssetManager assetManager;

    protected override void InitData()
    {
        if (localizationInfo == null)
        {
            AssetManager.Init(out assetManager);
            localizationInfo = new();
        }
    }

    protected override void SetData()
    {
        var language = GetEnum<SystemLanguage>("SystemLanguage");
        var resourcePath = GetString("AssetsPath");
        var localizationType = GetEnum<LocalizationType>("LocalizationType");
        var localization_FileDataVO = new Localization_FileDataVO(resourcePath, localizationType);
        if (!localizationInfo.TryGetValue(CsvName, out var languageDict))
        {
            var supportedLanguageCount = Hub.Localization.GetSupportedLanguageCount();
            languageDict = new Dictionary<SystemLanguage, List<Localization_FileDataVO>>(supportedLanguageCount);
            localizationInfo.Add(CsvName, languageDict);
        }
        if (!languageDict.TryGetValue(language, out var dataList))
        {
            dataList = new List<Localization_FileDataVO>(RowCount);
            languageDict.Add(language, dataList);
        }
        dataList.Add(localization_FileDataVO);
    }
    
    /// <summary>
    /// 加载资源信息_多语言数据表
    /// </summary>
    public async UniTask LoadInfo(List<Localization_FileDataVO> localization_FileDataVOs)
    {
        var assetsPath = new List<string>(localization_FileDataVOs.Count);
        foreach (var item in localization_FileDataVOs)
        {
            assetsPath.Add(item.resourcePath);
        }
        await assetManager.AddLoad(assetsPath);
    }

    /// <summary>
    /// 卸载资源信息_多语言数据表
    /// </summary>
    public void Release(TextAsset textAsset, SystemLanguage systemLanguage)
    {
        Release(GetLocalizationFileData(textAsset, systemLanguage));
    }

    /// <summary>
    /// 卸载资源信息_多语言数据表
    /// </summary>
    public void Release(List<Localization_FileDataVO> localization_FileDataVOs)
    {
        foreach (var item in localization_FileDataVOs)
        {
            assetManager.Release(item.resourcePath);
        }
    }

    /// <summary>
    /// 获取本地化文件信息
    /// </summary>
    public List<Localization_FileDataVO> GetLocalizationFileData(TextAsset textAsset, SystemLanguage systemLanguage)
    {
        return localizationInfo[textAsset.name][systemLanguage];
    }

    /// <summary>
    /// 是否已经加载过多语言文件路径数据
    /// </summary>
    public bool CheckIsParseData(TextAsset textAsset)
    {
        if (localizationInfo == null)
        {
            return false;
        }
        else
        {
            return localizationInfo.ContainsKey(textAsset.name);
        }
    }
}
