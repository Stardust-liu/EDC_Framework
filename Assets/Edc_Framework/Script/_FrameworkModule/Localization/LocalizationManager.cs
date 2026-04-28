using System.Collections;
using System.Collections.Generic;
using ArchiveData;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LocalizationManager : BaseIOCComponent<LanguageData>, ISendEvent
{
    private SystemLanguage CurrentLanguage{get{ return Data.currentLanguage;}}
    private LocalizationSetting localizationFontSetting;
    private Localization_FileData localization_FilePath;
    private Localization_TextInfoCfg localization_TextInfoCfg;
    private Localization_AssetInfoCfg localization_AssetInfoCfg;
    private Localization_SizeInfoCfg localization_SizeInfoCfg;
    public int supportedLanguageCount;
    private string[] localizationInfo;
    private SystemLanguage[] languageOrder;

    protected override void Init()
    {
        base.Init();
       
        localizationFontSetting = Hub.Resources.Get<LocalizationSetting>("LocalizationFontSetting");
        InitInfoCfg();
        SetInitLanguage();
    }

    private void InitInfoCfg()
    {
        var languageListCfg = Localization_languageListCfg.Instance.ParseData(localizationFontSetting.languageList);
        localization_FilePath = Localization_FileData.Instance;
        localization_TextInfoCfg = Localization_TextInfoCfg.Instance;
        localization_AssetInfoCfg = Localization_AssetInfoCfg.Instance;
        localization_SizeInfoCfg = Localization_SizeInfoCfg.Instance;
        localizationInfo = languageListCfg.localizationInfo;
        supportedLanguageCount = localizationInfo.Length;
        languageOrder = languageListCfg.languageOrder;
    }

    private void SetInitLanguage(){
        if(!Data.isInitSave){
            Data.ChangeLanguage(GetRecommendedLanguage());        
        }
    }

    /// <summary>
	/// 获取推荐语言
	/// </summary>
    /// 如果不支持设备语言首先会检查是否支持英文，如果不支持的话就返回中文
	public SystemLanguage GetRecommendedLanguage(){
        var systemLanguage = Application.systemLanguage;
        switch (Application.systemLanguage)
        {
            case SystemLanguage.Chinese:
            case SystemLanguage.ChineseSimplified:
                    return SystemLanguage.ChineseSimplified;
        }
        if(localizationFontSetting.CheckIsSupport(systemLanguage)){
            return systemLanguage;
        }
        else{
            if(localizationFontSetting.CheckIsSupport(SystemLanguage.English)){
                return SystemLanguage.English;
            }
            else{
                return SystemLanguage.ChineseSimplified;
            }
        }
	}

    /// <summary>
	/// 改变语言
	/// </summary>
	public void ChangeLanguage(SystemLanguage languageId)
    {
        if(Data.currentLanguage != languageId){
            CleanLocalizationData();
            this.SendEvent(new ReadyChangeLanguage(CurrentLanguage, languageId));
            Data.ChangeLanguage(languageId);
            this.SendEvent(new ChangeLanguage(languageId));
        }
	}
    
    /// <summary>
    /// 改变语言
    /// </summary>
    public void ChangeLanguage(int index)
    {
        var languageId = languageOrder[index];
        ChangeLanguage(languageId);
    }

    /// <summary>
    /// 获取当前语言
    /// </summary>
    public SystemLanguage GetCurrentLanguage(){
        return CurrentLanguage;
    }

    /// <summary>
    /// 获取当前语言索引
    /// </summary>
    public int GetCurrentLanguageIndex(){
        return System.Array.IndexOf(languageOrder, CurrentLanguage);
    }
    
    /// <summary>
    /// 获取支持语言的字符串信息
    /// </summary>
    public string[] GetLanguageInfo()
    {
        return localizationInfo;
    }

    /// <summary>
    /// 获取支持语言数量
    /// </summary>
    public int GetSupportedLanguageCount()
    {
        return supportedLanguageCount;
    }

    /// <summary>
    /// 添加数据
    /// </summary>
    public async UniTask AddLocalizationData(TextAsset localizationCsvFile, SystemLanguage systemLanguage)
    {
        if (!localization_FilePath.CheckIsParseData(localizationCsvFile))
        {
            localization_FilePath.ParseData(localizationCsvFile);
        }
        var localizationCsvData = localization_FilePath.GetLocalizationFileData(localizationCsvFile, systemLanguage);
        await localization_FilePath.LoadInfo(localizationCsvData);
        foreach (var item in localizationCsvData)
        {
            var csv = Hub.Resources.Get<TextAsset>(item.resourcePath);
            switch (item.localizationType)
            {
                case LocalizationType.Text:
                    AddTextInfo(csv);
                    break;
                case LocalizationType.Size:
                    AddSizeInfo(csv);
                    break;
                case LocalizationType.Asset:
                    await AddAssetInfo(csv);
                    break;
            }
        }
    }

    /// <summary>
    /// 移除数据
    /// </summary>
    public void RemoveLocalizationData(TextAsset localizationCsvFile, SystemLanguage systemLanguage)
    {
        var localizationCsvData = localization_FilePath.GetLocalizationFileData(localizationCsvFile, systemLanguage);
        foreach (var item in localizationCsvData)
        {
            var csv = Hub.Resources.Get<TextAsset>(item.resourcePath);
            switch (item.localizationType)
            {
                case LocalizationType.Text:
                    RemoveTextInfo(csv);
                    break;
                case LocalizationType.Size:
                    RemoveSizeInfo(csv);
                    break;
                case LocalizationType.Asset:
                    RemoveAssetInfo(csv);
                    break;
            }
        }
        localization_FilePath.Release(localizationCsvData);
    }

    /// <summary>
    /// 移除数据
    /// 调用前提：已经调用了全局的 CleanLocalizationData() 清空了所有字典数据。
    /// 场景：切换语言时，为了性能先清理资源信息，再由各组件触发此方法释放对应的文件
    /// </summary>
    public void ClearLocalizationData(TextAsset localizationCsvFile, SystemLanguage systemLanguage)
    {
        localization_FilePath.Release(localizationCsvFile, systemLanguage);
    }

    /// <summary>
    /// 清空数据
    /// </summary>
    public void CleanLocalizationData()
    {
        localization_TextInfoCfg.CleanLocalizationData();
        localization_AssetInfoCfg.CleanLocalizationData();
        localization_SizeInfoCfg.CleanLocalizationData();
    }

#region 文本信息
    /// <summary>
    /// 增加本地化文本信息
    /// </summary>
    private void AddTextInfo(TextAsset csv)
    {
        localization_TextInfoCfg.AddLocalizationAsset(csv);
    }

    /// <summary>
    /// 移除本地化文本信息
    /// </summary>
    private void RemoveTextInfo(TextAsset csv){
        localization_TextInfoCfg.RemoveLocalizationAsset(csv);
    }

    /// <summary>
    /// 获取本地化文字
    /// </summary>
    public string GetLocalizationText(string key){
        return localization_TextInfoCfg.GetLocalizationText(key);
    }

    /// <summary>
    /// 获取字体信息
    /// </summary>
    public FontSetting GetFontSetting(){
        return localizationFontSetting.GetFontSetting(CurrentLanguage);
    }
#endregion
#region Asset信息
    /// <summary>
    /// 增加本地化Asset信息
    /// </summary>
    private async UniTask AddAssetInfo(TextAsset csv){
        localization_AssetInfoCfg.AddLocalizationAsset(csv);
        await localization_AssetInfoCfg.LoadInfo();
    }

    /// <summary>
    /// 移除本地化Asset信息
    /// </summary>
    private void RemoveAssetInfo(TextAsset csv){
        localization_AssetInfoCfg.RemoveLocalizationAsset(csv);
    }

    /// <summary>
    /// 获取本地化Asset信息
    /// </summary>
    public T GetLocalizationAsset<T>(string key) where T : Object
    {
        return localization_AssetInfoCfg.GetLocalizationAsset<T>(key);
    }
#endregion
#region 尺寸信息
    /// <summary>
    /// 增加本地化尺寸信息
    /// </summary>
    private void AddSizeInfo(TextAsset csv){
        localization_SizeInfoCfg.AddLocalizationAsset(csv);
    }

    /// <summary>
    /// 移除本地化尺寸信息
    /// </summary>
    private void RemoveSizeInfo(TextAsset csv){
        localization_SizeInfoCfg.RemoveLocalizationAsset(csv);
    }

    /// <summary>
    /// 获取本地化尺寸
    /// </summary>
    public Vector2 GetLocalizationSize(string key){
        return localization_SizeInfoCfg.GetLocalizationSize(key);
    }
#endregion
}
