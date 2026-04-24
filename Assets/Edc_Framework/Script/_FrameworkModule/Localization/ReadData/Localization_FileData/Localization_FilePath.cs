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
    private Dictionary<SystemLanguage, List<Localization_FileDataVO>> localizationInfo;
    private Dictionary<SystemLanguage, List<string>> labelInfo;
    public LabelManager labelManager;
    protected override void InitData()
    {
        if (localizationInfo == null)
        {
            var count = Enum.GetValues(typeof(SystemLanguage)).Length;
            localizationInfo = new(count);
            labelInfo = new(count);
        }
        else
        {
            foreach (var item in localizationInfo)
            {
                item.Value.Clear();
            }
            foreach (var item in labelInfo)
            {
                item.Value.Clear();
            }
        }
    }

    protected override void SetData()
    {
        var resourcePath = GetString("AssetsPath");
        var localizationType = GetEnum<LocalizationType>("LocalizationType");
        var language = GetEnum<SystemLanguage>("SystemLanguage");
        var label = GetString("LabelName");
        var Localization_FileDataVO = new Localization_FileDataVO(resourcePath, localizationType);
        
        if (!localizationInfo.ContainsKey(language))
        {
            localizationInfo.Add(language, new List<Localization_FileDataVO>() { Localization_FileDataVO });
            labelInfo.Add(language, new List<string>() { label });
        }
        else
        {
            if (!labelInfo[language].Contains(label))
            {
                labelInfo[language].Add(label);
            }
            localizationInfo[language].Add(Localization_FileDataVO);
        }
    }
    
    /// <summary>
    /// 加载Label信息
    /// </summary>
    public async UniTask LoadLabelInfo(SystemLanguage systemLanguage)
    {
        await LabelManager.Init(out labelManager, labelInfo[systemLanguage]).LoadLabel();
    }

    /// <summary>
    /// 卸载Label信息
    /// </summary>
    public void ReleaseLabel(TextAsset textAsset)
    {
        labelManager.ReleaseLabelAll();
    }

    /// <summary>
    /// 获取本地化文件信息
    /// </summary>
    public List<Localization_FileDataVO> GetLocalizationFileData(SystemLanguage systemLanguage)
    {
        return localizationInfo[systemLanguage];
    }
}
