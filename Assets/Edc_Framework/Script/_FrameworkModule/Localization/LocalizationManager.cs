using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using ArchiveData;
using UnityEngine;

public enum LanguageId{
    zh_Hans = 0,
    zh_Hant = 1,
    en = 2,
    ja = 3,
    ko = 4,
}
public class LocalizationEventName{
    public const string changeLanguage = nameof(changeLanguage);
}

public class LocalizationManager
{
    public LanguageId CurrentLanguage{get; private set;}
    public int SupportLanguageCount{get; private set;}
    private readonly Dictionary<LanguageId , Dictionary<string, string>> localizationInfo;
    public static readonly EventCenter eventCenter = new EventCenter();
    private readonly LanguageData languageData;
    private readonly LocalizationInfoCfg localizationInfoCfg;

    public LocalizationManager(){
        localizationInfoCfg = LocalizationInfoCfg.Instance;
        languageData = GameArchive.LanguageData;
        var languageIdArray = Enum.GetValues(typeof(LanguageId));
        SupportLanguageCount = languageIdArray.Length;
        foreach (LanguageId item in languageIdArray)
        {
            localizationInfo.Add(item, new Dictionary<string, string>());
        }
        localizationInfoCfg.Init(localizationInfo);
        SetInitLanguage();
	}

    private void SetInitLanguage(){
        if(languageData.isInitSave){
            CurrentLanguage = languageData.currentLanguage;
        }
        else{
            var recommendedLanguage = GetRecommendedLanguage();
            CurrentLanguage = recommendedLanguage;
            languageData.ChangeLanguage(recommendedLanguage);
        }
    }


    /// <summary>
	/// 获取推荐语言
	/// </summary>
	public LanguageId GetRecommendedLanguage(){
        LanguageId recommendedLanguage;
        switch (Application.systemLanguage)
        {
            case SystemLanguage.Chinese:
            case SystemLanguage.ChineseSimplified:
                    recommendedLanguage = LanguageId.zh_Hans;
                break;
            case SystemLanguage.ChineseTraditional:
                    recommendedLanguage = LanguageId.zh_Hans;
                break;
            case SystemLanguage.English:
                    recommendedLanguage = LanguageId.en;
                break;
            case SystemLanguage.Japanese:
                    recommendedLanguage = LanguageId.ja;
                break;
            case SystemLanguage.Korean:
                    recommendedLanguage = LanguageId.ko;
                break;
            default:
                    recommendedLanguage = LanguageId.en;
                break;
        }
        return recommendedLanguage;
	}

    /// <summary>
	/// 改变语言
	/// </summary>
	public void ChangeLange(LanguageId languageId)
    {
		eventCenter.OnEvent(LocalizationEventName.changeLanguage, languageId);
		CurrentLanguage = languageId;
        languageData.ChangeLanguage(languageId);
	}

    /// <summary>
    /// 增加本地化资源
    /// </summary>
    public void AddLocalizationAsset(TextAsset csv){
        LocalizationInfoCfg.Instance.AddLocalizationAsset(csv);
    }

    /// <summary>
    /// 移除本地化资源
    /// </summary>
    public void RemoveLocalizationAsset(TextAsset csv){
        LocalizationInfoCfg.Instance.RemoveLocalizationAsset(csv);
    }

    /// <summary>
    /// 获取本地化文字
    /// </summary>
    public string GetLocalizationText(string key){
        return localizationInfo[CurrentLanguage][key];
    }
}
