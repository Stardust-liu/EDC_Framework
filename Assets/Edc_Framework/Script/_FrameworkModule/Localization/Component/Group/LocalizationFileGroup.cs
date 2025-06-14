using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class LocalizationFileGroup : MonoBehaviour, IAutoBindEvent
{
    [LabelText("本地化文件列表")]
    public TextAsset[] localizationCsvFiles;

    private void Awake()
    {
        if (FrameworkManager.isInitFinish)
        {
            this.AddListener_EnableDisable<ReadyChangeLanguage>(ChangeLanguage, gameObject);
            AddLocalizationData(Hub.Localization.GetCurrentLanguage());
        }
    }

    private void OnDestroy()
    {
#if !UNITY_EDITOR
        if (FrameworkManager.isInitFinish)
        {
            RemoveLocalizationData(Hub.Localization.GetCurrentLanguage());
        }
#endif
    }

    private void ChangeLanguage(ReadyChangeLanguage readyChangeLanguage)
    {
        AddLocalizationData(readyChangeLanguage.tagetLanguageId);
    }

    /// <summary>
    /// 添加数据
    /// </summary>
    private void AddLocalizationData(SystemLanguage systemLanguage)
    {
        var localization = Hub.Localization;
        foreach (var item in localizationCsvFiles)
        {
            localization.AddLocalizationData(item, systemLanguage);
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
}
