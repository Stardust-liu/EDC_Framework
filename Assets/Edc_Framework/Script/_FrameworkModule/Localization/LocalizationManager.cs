using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using ArchiveData;
using UnityEngine;

public enum LanguageId{
    zh_Hans = 0,//简体中文
    zh_Hant = 1,//繁体中文
    en = 2,//英语
    ja = 3,//日语
    ko = 4,//韩语
}
public class LocalizationEventName{
    public const string changeLanguage = nameof(changeLanguage);
}

public class LocalizationManager : BaseIOCComponent
{
    public LanguageId CurrentLanguage{get{ return data.currentLanguage;}}
    public LanguageId[] LanguageId{get; private set;}
    private LocalizationFontSetting localizationFontSetting;
    private Localization_TextInfoCfg localization_TextInfoCfg;
    private Localization_AudioInfoCfg localization_AudioInfoCfg;
    private Localization_ImageInfoCfg localization_ImageInfoCfg;
    private Localization_SizeInfoCfg localization_SizeInfoCfg;
    private static ResourcesModule resourcesModule = Hub.Resources;
    private LanguageData data;
    public static readonly EventCenter eventCenter = new EventCenter();

    protected override void Init()
    {
        data = GameArchive.LanguageData;
        LanguageId = (LanguageId[])Enum.GetValues(typeof(LanguageId));
        localizationFontSetting = Hub.Resources.GetScriptableobject<LocalizationFontSetting>(nameof(LocalizationFontSetting));
        InitInfoCfg();
        SetInitLanguage();    
    }

    private void InitInfoCfg(){
        var supportLanguageCount = LanguageId.Length;
        localization_TextInfoCfg = Localization_TextInfoCfg.Instance;
        localization_AudioInfoCfg = Localization_AudioInfoCfg.Instance;
        localization_ImageInfoCfg = Localization_ImageInfoCfg.Instance;
        localization_SizeInfoCfg = Localization_SizeInfoCfg.Instance;
        localization_TextInfoCfg.Init(LanguageId, supportLanguageCount);
        localization_AudioInfoCfg.Init(LanguageId, supportLanguageCount);
        localization_ImageInfoCfg.Init(LanguageId, supportLanguageCount);
        localization_SizeInfoCfg.Init(LanguageId, supportLanguageCount);

    }

    private void SetInitLanguage(){
        if(!data.isInitSave){
            data.ChangeLanguage(GetRecommendedLanguage());        
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
                    recommendedLanguage = global::LanguageId.zh_Hans;
                break;
            case SystemLanguage.ChineseTraditional:
                    recommendedLanguage = global::LanguageId.zh_Hans;
                break;
            case SystemLanguage.English:
                    recommendedLanguage = global::LanguageId.en;
                break;
            case SystemLanguage.Japanese:
                    recommendedLanguage = global::LanguageId.ja;
                break;
            case SystemLanguage.Korean:
                    recommendedLanguage = global::LanguageId.ko;
                break;
            default:
                    recommendedLanguage = global::LanguageId.en;
                break;
        }
        return recommendedLanguage;
	}

    /// <summary>
	/// 改变语言
	/// </summary>
	public void ChangeLange(LanguageId languageId)
    {
        if(data.currentLanguage != languageId){
            data.ChangeLanguage(languageId);
            eventCenter.OnEvent(LocalizationEventName.changeLanguage, languageId);
        }
	}
#region 文本资源
    /// <summary>
    /// 增加本地化文本资源
    /// </summary>
    public void AddTextAsset(TextAsset csv){
        localization_TextInfoCfg.AddLocalizationAsset(csv);
    }

    /// <summary>
    /// 移除本地化文本资源
    /// </summary>
    public void RemoveTextAsset(TextAsset csv){
        localization_TextInfoCfg.RemoveLocalizationAsset(csv);
    }

    /// <summary>
    /// 获取本地化文字
    /// </summary>
    public string GetLocalizationText(string key){
        return localization_TextInfoCfg.GetLocalizationText(CurrentLanguage ,key);
    }

    /// <summary>
    /// 获取字体信息
    /// </summary>
    public FontSetting GetFontSetting(){
        return localizationFontSetting.fontSetting[CurrentLanguage];
    }
#endregion
#region 音频资源
    /// <summary>
    /// 增加本地化音频资源
    /// </summary>
    public void AddAudioAsset(TextAsset csv){
        localization_AudioInfoCfg.AddLocalizationAsset(csv);
    }

    /// <summary>
    /// 移除本地化音频资源
    /// </summary>
    public void RemoveAudioAsset(TextAsset csv){
        localization_AudioInfoCfg.RemoveLocalizationAsset(csv);
    }

    /// <summary>
    /// 获取本地化音频
    /// </summary>
    public AudioClip GetLocalizationAudio(string key, AudioType audioType){
        var path = localization_AudioInfoCfg.GetLocalizationAudio(CurrentLanguage ,key);
        switch (audioType)
        {
            case AudioType.SoundBg:
                return resourcesModule.GetSoundBg(path);
            case AudioType.SoundEffect:
                return resourcesModule.GetSoundEffect(path);
            case AudioType.SoundDialogue:
                return resourcesModule.GetSoundDialogue(path);
        }
        return null;
    }
#endregion 
#region 图像资源
    /// <summary>
    /// 增加本地化图像资源
    /// </summary>
    public void AddImageAsset(TextAsset csv){
        localization_ImageInfoCfg.AddLocalizationAsset(csv);
    }

    /// <summary>
    /// 移除本地化图像资源
    /// </summary>
    public void RemoveImageAsset(TextAsset csv){
        localization_ImageInfoCfg.RemoveLocalizationAsset(csv);
    }

    /// <summary>
    /// 获取本地化图像
    /// </summary>
    public Sprite GetLocalizationImage(string key){
        var path = localization_ImageInfoCfg.GetLocalizationImage(CurrentLanguage ,key);
        return resourcesModule.GetSprite(path);
    }
#endregion
#region 尺寸资源
/// <summary>
    /// 增加本地化尺寸资源
    /// </summary>
    public void AddSizeAsset(TextAsset csv){
        localization_SizeInfoCfg.AddLocalizationAsset(csv);
    }

    /// <summary>
    /// 移除本地化尺寸资源
    /// </summary>
    public void RemoveSizeAsset(TextAsset csv){
        localization_SizeInfoCfg.RemoveLocalizationAsset(csv);
    }

    /// <summary>
    /// 获取本地化尺寸
    /// </summary>
    public Vector2 GetLocalizationSize(string key){
        return localization_SizeInfoCfg.GetLocalizationSize(CurrentLanguage ,key);
    }
#endregion
}
