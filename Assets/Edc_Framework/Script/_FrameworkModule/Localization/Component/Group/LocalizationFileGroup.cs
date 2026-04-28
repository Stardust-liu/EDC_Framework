using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public class LocalizationFileGroup : MonoBehaviour, IAutoBindEvent
{
    [LabelText("本地化文件列表")]
    public TextAsset[] localizationCsvFiles;
    public LocalizationGroup[] localizationGroups;
    public bool isAutoLoadData;
    private SystemLanguage currentLanguage;
    private void Awake()
    {
        if (FrameworkManager.isInitFinish && isAutoLoadData)
        {
            LoadInfo().Forget();
        }
    }

    private void OnDestroy()
    {
        if (FrameworkManager.isInitFinish)
        {
            RemoveLocalizationData(currentLanguage);
        }
    }

    public async UniTask LoadInfo()
    {
        currentLanguage = Hub.Localization.GetCurrentLanguage();
        this.AddListener_EnableDisable<ReadyChangeLanguage>(ChangeLanguage, gameObject);
        await AddLocalizationData(currentLanguage);
        LoadComplete();
    }

    private async void ChangeLanguage(ReadyChangeLanguage readyChangeLanguage)
    {
        Reset();
        ClearLocalizationData(currentLanguage);
        await AddLocalizationData(readyChangeLanguage.tagetLanguageId);
        currentLanguage = Hub.Localization.GetCurrentLanguage();
        LoadComplete();
    }

    /// <summary>
    /// 添加数据
    /// </summary>
    private async UniTask AddLocalizationData(SystemLanguage systemLanguage)
    {
        var localization = Hub.Localization;
        foreach (var item in localizationCsvFiles)
        {
            await localization.AddLocalizationData(item, systemLanguage);
        }
    }

    /// <summary>
    /// 移除数据
    /// </summary>
    private void RemoveLocalizationData(SystemLanguage systemLanguage)
    {
        var localization = Hub.Localization;
        foreach (var item in localizationCsvFiles)
        {
            localization.RemoveLocalizationData(item, systemLanguage);
        }
    }

    /// <summary>
    /// 清除数据
    /// </summary>
    private void ClearLocalizationData(SystemLanguage systemLanguage)
    {
        var localization = Hub.Localization;
        foreach (var item in localizationCsvFiles)
        {
            localization.ClearLocalizationData(item, systemLanguage);
        }
    }

    /// <summary>
    /// 重置加载状态
    /// </summary>
    private void Reset()
    {
        foreach (var item in localizationGroups)
        {
            item.Reset();
        }
    }

    /// <summary>
    /// 加载数据完成
    /// </summary>
    private void LoadComplete()
    {
        foreach (var item in localizationGroups)
        {
            item.Init();
            item.RefreshContent();
        }
    }
}
