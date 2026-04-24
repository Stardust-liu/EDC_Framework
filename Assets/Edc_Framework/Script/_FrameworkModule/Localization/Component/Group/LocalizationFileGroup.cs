using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public class LocalizationFileGroup : MonoBehaviour, IAutoBindEvent
{
    [LabelText("本地化文件列表")]
    public TextAsset[] localizationCsvFiles;
    public LocalizationGroup[] localizationGroups;
    public bool isAutoLoadData;
    private SystemLanguage currentLoadLanguage;
    private void Awake()
    {
        if (FrameworkManager.isInitFinish && isAutoLoadData)
        {
            Loadnfo().Forget();
        }
    }

    private void OnDestroy()
    {
        if (FrameworkManager.isInitFinish)
        {
            RemoveLocalizationData(currentLoadLanguage);
        }
    }

    public async UniTask Loadnfo()
    {
        currentLoadLanguage = Hub.Localization.GetCurrentLanguage();
        this.AddListener_EnableDisable<ReadyChangeLanguage>(ChangeLanguage, gameObject);
        await AddLocalizationData(currentLoadLanguage);
        LoadComplete();
    }

    private async void ChangeLanguage(ReadyChangeLanguage readyChangeLanguage)
    {
        Reset();
        RemoveLocalizationData(currentLoadLanguage);
        currentLoadLanguage = readyChangeLanguage.tagetLanguageId;
        await AddLocalizationData(currentLoadLanguage);
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

    /// <summary>
    /// 移除数据
    /// </summary>
    private void RemoveLocalizationData(SystemLanguage systemLanguage)
    {
        var localization = Hub.Localization;
        foreach (var item in localizationCsvFiles)
        {
            localization.RemoveLocalizationData(item, systemLanguage).Forget();
        }
    }
}
